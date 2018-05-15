using UnityEngine;

namespace Shoooooo
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Bullet : MonoBehaviour
    {
        private static readonly Vector2 Range = new Vector2(56, 100);

        public Vector2 Velocity { get; private set; }

        private Transform cachedTransform;

        void Awake()
        {
            this.cachedTransform = this.transform;
        }

        public void Initialize(Vector2 velocity)
        {
            this.Velocity = velocity;
        }

        public void UpdatePosition()
        {
            var position = this.cachedTransform.localPosition;
            position.x += this.Velocity.x;
            position.y += this.Velocity.y;
            this.cachedTransform.localPosition = position;
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
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
