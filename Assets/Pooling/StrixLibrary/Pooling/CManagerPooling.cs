﻿#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2018-12-21 오전 10:13:20
 *	기능 : 
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CManagerPooling<Class_GetType> : CManagerPoolingBase<CManagerPooling<Class_GetType>, Class_GetType>
    where Class_GetType : class, new()
{
    protected override Class_GetType OnCreateClass_WhenEmptyPool(Class_GetType pObjectCopyTarget, int iID)
    {
        return new Class_GetType();
    }
}

public abstract class CManagerPoolingBase<Class_Driven, Class_GetType> : CSingletonNotMonoBase<Class_Driven>
    where Class_Driven : CManagerPoolingBase<Class_Driven, Class_GetType>, new()
    where Class_GetType : class
{
    /* const & readonly declaration             */

    /* enum & struct declaration                */

    public class IntComparer : EqualityComparer<int>
    {
        public override bool Equals(int x, int y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }
    }

    /* public - Field declaration            */

    public bool p_bIsDebug = false;

    public int p_iInstanceCount { get { return _mapAllInstance.Count; } }

    /* protected & private - Field declaration         */

    protected Dictionary<Class_GetType, int> _mapAllInstance = new Dictionary<Class_GetType, int>();

    // 본래 LinkedList를 사용했으나, C#에선 LinkedList가 오히려 더 느리다..
    // https://stackoverflow.com/questions/5983059/why-is-a-linkedlist-generally-slower-than-a-list

    protected Dictionary<int, HashSet<Class_GetType>> _mapUsed = new Dictionary<int, HashSet<Class_GetType>>(new IntComparer());
    protected Dictionary<int, List<Class_GetType>> _mapUnUsed = new Dictionary<int, List<Class_GetType>>(new IntComparer());
    
    // ========================================================================== //

    /* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

    public void DoPrePooling(Class_GetType pObjectCopyTarget, int iCount)
    {
        if (pObjectCopyTarget == null)
            return;

        int iID = pObjectCopyTarget.GetHashCode();
        Add_NewObjectType(pObjectCopyTarget, iID);

        int iTotalCount = _mapUnUsed[iID].Count + _mapUnUsed[iID].Count;
        if (iTotalCount > iCount)
            return;

        LinkedList<Class_GetType> listTemp = new LinkedList<Class_GetType>();
        int iPoolingCount = iCount - iTotalCount;
        for (int i = 0; i < iPoolingCount; i++)
            listTemp.AddLast(DoPop(pObjectCopyTarget));

        foreach (var pPrePoolingObject in listTemp)
            DoPush(pPrePoolingObject);
    }

    public Class_GetType DoPop(Class_GetType pObjectCopyTarget)
    {
        if (pObjectCopyTarget == null)
            return null;

        int iID = pObjectCopyTarget.GetHashCode();
        Add_NewObjectType(pObjectCopyTarget, iID);

        Class_GetType pUnUsed = Get_UnusedObject(pObjectCopyTarget, iID);
        if (p_bIsDebug)
            Debug.Log(name + " Pooling Simple Pop - " + pUnUsed.ToString());

        OnPopObject(pUnUsed);

        return pUnUsed;
    }


    public void DoPush(GameObject pObjectReturn)
    {
        DoPush(pObjectReturn.GetComponent<Class_GetType>());
    }

    public void DoPush(Class_GetType pClassType)
    {
        if (pClassType == null)
            return;

        if (_mapAllInstance.ContainsKey(pClassType) == false)
            return;

        Remove_UsedList(pClassType);

        if (p_bIsDebug)
            Debug.Log("Pooling Simple Pushed - " + pClassType, this);

        OnPushObject(pClassType);
    }

    public void DoPushAll()
    {
        foreach (var pObject in _mapAllInstance.Keys)
            DoPush(pObject);
    }

    virtual public void DoDestroyAll()
    {
        _mapAllInstance.Clear();
        _mapUsed.Clear();
        _mapUnUsed.Clear();
    }

    public void Event_RemovePoolObject(GameObject pObjectDestroyed)
    {
        if (pObjectDestroyed == null)
            return;

        Class_GetType pObjectReturn = pObjectDestroyed.GetComponent<Class_GetType>();
        if (pObjectReturn == null || _mapAllInstance.ContainsKey(pObjectReturn) == false)
            return;

        int iID = _mapAllInstance[pObjectReturn];
        if (_mapUsed.ContainsKey(iID) && _mapUsed[iID].Contains(pObjectReturn))
            _mapUsed[iID].Remove(pObjectReturn);

        if (_mapUnUsed.ContainsKey(iID) && _mapUnUsed[iID].Contains(pObjectReturn))
            _mapUnUsed[iID].Remove(pObjectReturn);

        _mapAllInstance.Remove(pObjectReturn);
    }

    // ========================================================================== //

    /* protected - Override & Unity API         */

    protected override void OnMakeSingleton(out bool bIsGenearteGameObject, out bool bIsUpdateAble)
    {
        bIsGenearteGameObject = true;
#if UNITY_EDITOR
        bIsUpdateAble = true;
        strTypeName = typeof(Class_GetType).Name;
#else
        bIsUpdateAble = false;
#endif
    }

    protected override void OnMakeGameObject(GameObject pObject)
    {
        base.OnMakeGameObject(pObject);

        DontDestroyOnLoad(pObject);
    }

    protected override void OnSceneUnloaded(UnityEngine.SceneManagement.Scene pScene)
    {
        base.OnSceneUnloaded(pScene);

        DoDestroyAll();
    }

    string strTypeName;

#if UNITY_EDITOR // 하이어라키뷰에 실시간 풀링 상황 모니터링을 위한 Update

    public override void OnUpdate()
    {
        base.OnUpdate();

        int iUseCount = 0;
        foreach (var pList in _mapUsed.Values)
            iUseCount += pList.Count;

        gameObject.name = string.Format("풀링<{0}>/ 총생산:{1} /사용중:{2}", strTypeName, _mapAllInstance.Count, iUseCount);
    }

#endif

    /* protected - [abstract & virtual]         */

    abstract protected Class_GetType OnCreateClass_WhenEmptyPool(Class_GetType pObjectCopyTarget, int iID);

    virtual protected void OnPopObject(Class_GetType pClassType) { }
    virtual protected void OnPushObject(Class_GetType pClassType) { }

    // ========================================================================== //

    #region Private

    private void Add_NewObjectType(Class_GetType pObjectCopyTarget, int iID)
    {
        if (_mapUnUsed.ContainsKey(iID) == false)
        {
            _mapUsed.Add(iID, new HashSet<Class_GetType>());
            _mapUnUsed.Add(iID, new List<Class_GetType>());
        }
    }

    private Class_GetType Get_UnusedObject(Class_GetType pObjectCopyTarget, int iID)
    {
        Class_GetType pComponentUnUsed;
        if (_mapUnUsed[iID].Count != 0)
        {
            int iIndexLast = _mapUnUsed[iID].Count - 1;
            pComponentUnUsed = _mapUnUsed[iID][iIndexLast];
            _mapUnUsed[iID].RemoveAt(iIndexLast);
        }
        else
        {
            pComponentUnUsed = OnCreateClass_WhenEmptyPool(pObjectCopyTarget, iID);

            try
            {
                _mapAllInstance.Add(pComponentUnUsed, iID);
            }
            catch
            {
                _mapAllInstance.Add(pComponentUnUsed, iID);
            }
        }

        _mapUsed[iID].Add(pComponentUnUsed);
        return pComponentUnUsed;
    }

    private void Remove_UsedList(Class_GetType pObjectReturn)
    {
        int iID = _mapAllInstance[pObjectReturn];
        if (_mapUsed.ContainsKey(iID) && _mapUsed[iID].Contains(pObjectReturn))
        {
            _mapUsed[iID].Remove(pObjectReturn);

            if (_mapUnUsed.ContainsKey(iID))
                _mapUnUsed[iID].Add(pObjectReturn);
        }
    }

    #endregion Private
}