using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Shoooooo.ObjectPools
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ObjectPoolBundle<T> where T : Component
    {
        private readonly Dictionary<T, ObjectPool<T>> pools = new Dictionary<T, ObjectPool<T>>();

        public ObjectPool<T> Get(T original)
        {
            ObjectPool<T> result;
            if (!pools.TryGetValue(original, out result))
            {
                result = new ObjectPool<T>(original);
                pools.Add(original, result);
            }

            return result;
        }
    }
}
