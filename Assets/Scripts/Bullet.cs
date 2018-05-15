using Shoooooo.ObjectPools;
using UnityEngine;

namespace Shoooooo
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Bullet : MonoBehaviour
    {
        private static readonly Vector2 Range = new Vector2(56, 100);

        private Transform cachedTransform;

        private ObjectPool<Bullet> pool;

        void Awake()
        {
            this.cachedTransform = this.transform;
        }

        public void Initialize(ObjectPool<Bullet> pool)
        {
            this.pool = pool;
        }

        public void ReturnToPool()
        {
            this.pool.Return(this);
        }

        public void UpdatePosition(Vector2 velocity)
        {
            var position = this.cachedTransform.localPosition;
            position.x += velocity.x;
            position.y += velocity.y;
            this.cachedTransform.localPosition = position;
        }

        public bool CanDestroy
        {
            get
            {
                var position = this.cachedTransform.localPosition;
                return position.x < -Range.x || position.x > Range.x || position.y < -Range.y || position.y > Range.y;
            }
        }
    }
}
