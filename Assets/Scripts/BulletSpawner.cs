using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Shoooooo
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        private Bullet bulletPrefab;

        [SerializeField]
        private int interval;

        [SerializeField]
        private float addAxis;

        [SerializeField]
        private float bulletSpeed;

        private Transform cachedTransform;

        void Awake()
        {
            this.cachedTransform = this.transform;
            Observable.IntervalFrame(this.interval)
                .SubscribeWithState(this, (_, _this) =>
                {
                    _this.CreateBullet();
                    _this.cachedTransform.localRotation *= Quaternion.AngleAxis(_this.addAxis, Vector3.forward);
                })
                .AddTo(this);
        }

        private void CreateBullet()
        {
            BulletManager.Instance.CreateBullet(this.bulletPrefab).Initialize(this.cachedTransform.right * this.bulletSpeed);
        }
    }
}
