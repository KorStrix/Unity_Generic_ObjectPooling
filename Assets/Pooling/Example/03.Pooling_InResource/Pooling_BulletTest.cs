using UnityEngine;

namespace StrixLibrary_Example
{
    public class Pooling_BulletTest : MonoBehaviour
    {
        public enum ProjectileModel
        {
            Blue,
            Red
        }

        public float _fSpeed = 10;

        public void DoInitBullet(Transform pTransMuzzle)
        {
            transform.position = pTransMuzzle.position;
            transform.rotation = pTransMuzzle.rotation;
        }

        void OnEnable()
        {
            Invoke("DisableObject", 1f);
        }

        void Update()
        {
            transform.Translate(0, 0, _fSpeed * Time.deltaTime, Space.Self);
        }

        void OnDisable()
        {
            CManagerPooling_InResources<ProjectileModel, Pooling_BulletTest>.instance.DoPush(this);
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}