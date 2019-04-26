using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrixLibrary_Example
{
    public class Pooling_NotInherit_AnyClass : MonoBehaviour
    {
        [System.Serializable] // 인스펙터에 보이기 위함
        public class NotInheritClass
        {
            public string strText;
        }

        public List<NotInheritClass> listCurrentUsing = new List<NotInheritClass>();
        NotInheritClass _pNotInheritClass_Original = new NotInheritClass();

        private void Awake()
        {
            CManagerPooling<NotInheritClass>.instance.DoPrePooling(_pNotInheritClass_Original, 10);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Pop();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Push();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PushAll();
            }

        }

        private void Pop()
        {
            NotInheritClass pNotInheritClass = CManagerPooling<NotInheritClass>.instance.DoPop(_pNotInheritClass_Original);
            pNotInheritClass.strText = listCurrentUsing.Count.ToString();
            Debug.Log("Pop " + pNotInheritClass.strText);

            listCurrentUsing.Add(pNotInheritClass);
        }

        private void Push()
        {
            if (listCurrentUsing.Count != 0)
            {
                NotInheritClass pReturn = listCurrentUsing[listCurrentUsing.Count - 1];
                Debug.Log("Push " + pReturn.strText);

                listCurrentUsing.RemoveAt(listCurrentUsing.Count - 1);
                CManagerPooling<NotInheritClass>.instance.DoPush(pReturn);
            }
        }

        private void PushAll()
        {
            Debug.Log("Push All");
            listCurrentUsing.Clear();
            CManagerPooling<NotInheritClass>.instance.DoPushAll();
        }
    }
}