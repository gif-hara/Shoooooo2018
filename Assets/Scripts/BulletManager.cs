using System.Collections.Generic;
using Shoooooo.ObjectPools;
using UniRx.Toolkit;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace Shoooooo
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BulletManager : MonoBehaviour
    {
        public enum UpdateType
        {
            Basic,
            JobSystem,
        }

        private static BulletManager instance = null;
        public static BulletManager Instance
        {
            get { return instance; }
        }

        [SerializeField]
        private UpdateType updateType;
        
        private readonly List<Bullet> bullets = new List<Bullet>();

        private readonly List<Transform> bulletTransforms = new List<Transform>();
        
        private readonly List<Vector2> bulletVelocities = new List<Vector2>();

        private Transform cachedTransform;

        private JobHandle jobHandle;
        
        private readonly ObjectPoolBundle<Bullet> bulletPoolBundle = new ObjectPoolBundle<Bullet>();

        void Awake()
        {
            instance = this;
            this.cachedTransform = this.transform;
            Application.targetFrameRate = 60;
        }

        void Update()
        {
            switch (this.updateType)
            {
                case UpdateType.Basic:
                    this.UpdateBasic();
                    break;
                case UpdateType.JobSystem:
                    this.UpdateJobSystem();
                    break;
            }
        }

        private void UpdateBasic()
        {
            for (int i = 0; i < this.bullets.Count; ++i)
            {
                var bullet = this.bullets[i];
                var velocity = this.bulletVelocities[i];
                bullet.UpdatePosition(velocity);
            }
            
            this.DestroyBullet();
        }

        private void UpdateJobSystem()
        {
            this.jobHandle.Complete();
            
            var updater = new BulletUpdater(this.bulletVelocities.ToArray());
            var transformAccessArray = new TransformAccessArray(this.bulletTransforms.ToArray());
            this.jobHandle = updater.Schedule(transformAccessArray);
            JobHandle.ScheduleBatchedJobs();
            transformAccessArray.Dispose();
            updater.Dispose();
            this.DestroyBullet();
        }

        private void DestroyBullet()
        {
            for (int i = 0; i < this.bullets.Count; i++)
            {
                var bullet = this.bullets[i];
                if (bullet.CanDestroy)
                {
                    this.Remove(i);
                    --i;
                }
            }
        }

        private void Remove(int index)
        {
            var bullet = this.bullets[index];
            bullet.ReturnToPool();
            this.bullets.RemoveAt(index);
            this.bulletTransforms.RemoveAt(index);
            this.bulletVelocities.RemoveAt(index);
        }

        public Bullet CreateBulletElement(Bullet prefab, Vector2 position, Vector2 velocity)
        {
            var pool = this.bulletPoolBundle.Get(prefab);
            var bullet = pool.Rent();
            bullet.transform.position = position;
            bullet.Initialize(pool);
            bullet.transform.SetParent(this.cachedTransform);
            this.bullets.Add(bullet);
            this.bulletTransforms.Add(bullet.transform);
            this.bulletVelocities.Add(velocity);

            return bullet;
        }
    }
}
