using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Shoooooo
{
    /// <summary>
    /// 
    /// </summary>
    struct BulletUpdater : IJobParallelForTransform, IDisposable
    {
        private NativeArray<Vector2> velocities;
        
        public BulletUpdater(Vector2[] velocities)
        {
            this.velocities = new NativeArray<Vector2>(velocities, Allocator.TempJob);
        }
        
        public void Execute(int index, TransformAccess transform)
        {
            var position = transform.localPosition;
            var velocity = velocities[index];
            position.x += velocity.x;
            position.y += velocity.y;
            transform.localPosition = position;
        }

        public void Dispose()
        {
            velocities.Dispose();
        }
    }
}
