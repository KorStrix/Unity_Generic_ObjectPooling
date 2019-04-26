using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrixLibrary_Example
{
    public class Pooling_Transforms : MonoBehaviour
    {
        [SerializeField]
        private Transform _pTransMuzzle = null;

        public Transform pTransform_Blue;
        public Rigidbody pRigidbody_Red;

        void Start()
        {
            CManagerPooling_Component<Transform>.instance.DoPrePooling(pTransform_Blue, 10);
            CManagerPooling_Component<Rigidbody>.instance.DoPrePooling(pRigidbody_Red, 10);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Transform pTransform = CManagerPooling_Component<Transform>.instance.DoPop(pTransform_Blue);
                Rigidbody pRigidbody = pTransform.GetComponent<Rigidbody>();

                pTransform.position = _pTransMuzzle.transform.position;
                pRigidbody.AddForce(new Vector3(0, 10f, 0), ForceMode.VelocityChange);

                StartCoroutine(CoAutoDisable_Transform(pRigidbody.gameObject));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Rigidbody pRigidbody = CManagerPooling_Component<Rigidbody>.instance.DoPop(pRigidbody_Red);

                pRigidbody.transform.position = _pTransMuzzle.transform.position;
                pRigidbody.AddForce(new Vector3(0, 10f, 0), ForceMode.VelocityChange);

                StartCoroutine(CoAutoDisable_Rigidbody(pRigidbody.gameObject));
            }
        }

        IEnumerator CoAutoDisable_Transform(GameObject pObjectDisableTarget)
        {
            yield return new WaitForSeconds(3f);

            CManagerPooling_Component<Transform>.instance.DoPush(pObjectDisableTarget);
        }

        IEnumerator CoAutoDisable_Rigidbody(GameObject pObjectDisableTarget)
        {
            yield return new WaitForSeconds(3f);

            CManagerPooling_Component<Rigidbody>.instance.DoPush(pObjectDisableTarget);
        }
    }

}