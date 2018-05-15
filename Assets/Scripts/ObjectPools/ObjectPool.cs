using UnityEngine;

namespace Shoooooo.ObjectPools
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectPool<T> : UniRx.Toolkit.ObjectPool<T>
        where T : Component
    {
        private readonly T original;
        
        public ObjectPool(T original)
        {
            this.original = original;
        }

        protected override T CreateInstance()
        {
            return Object.Instantiate(this.original);
        }
    }
}
