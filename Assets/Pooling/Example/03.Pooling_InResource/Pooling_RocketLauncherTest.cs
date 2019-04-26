using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrixLibrary_Example
{
    public class Pooling_RocketLauncherTest : MonoBehaviour
    {
        [SerializeField]
        private Transform _pTransMuzzle = null;

        void Start()
        {
            CManagerPooling_InResources<string, Pooling_RocketTest>.instance.DoInitPoolingObject("PoolingTest_Rocket");
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Pooling_RocketTest pRocket = CManagerPooling_InResources<string, Pooling_RocketTest>.instance.DoPop("Blue");
                pRocket.DoInitRocket(_pTransMuzzle, 10);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Pooling_RocketTest pRocket = CManagerPooling_InResources<string, Pooling_RocketTest>.instance.DoPop("Red");
                pRocket.DoInitRocket(_pTransMuzzle, 20);
            }
        }
    }
}
