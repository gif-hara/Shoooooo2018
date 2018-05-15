using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;

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

        private static readonly List<Bullet> bullets = new List<Bullet>();

        private Predicate<Bullet> destroyBulletPredicate;

        void Awake()
        {
            instance = this;
            this.destroyBulletPredicate = b =>
            {
                if (b.CanDestroy)
                {
                    b.Destroy();
                    return true;
                }
                
                return false;
            };
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
            foreach (var bullet in bullets)
            {
                bullet.UpdatePosition();
            }

            bullets.RemoveAll(this.destroyBulletPredicate);
        }

        private void UpdateJobSystem()
        {
            
        }

        public Bullet CreateBullet(Bullet prefab)
        {
            var bullet = Instantiate(prefab);
            bullets.Add(bullet);

            return bullet;
        }

        public void Remove(Bullet bullet)
        {
            bullets.Remove(bullet);
            Destroy(bullet.gameObject);
        }
    }
}
