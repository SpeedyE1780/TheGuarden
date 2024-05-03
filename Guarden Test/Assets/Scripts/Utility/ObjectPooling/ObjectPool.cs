using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public abstract class ObjectPool : ScriptableObject
    {
    }

    public abstract class ObjectPool<T> : ObjectPool
    {
        private List<T> pooledObjects = new List<T>();

        public T GetPooledObject()
        {
            T pooledObject = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            return pooledObject;
        }

        public void AddObject(T obj)
        {
            pooledObjects.Add(obj);
        }
    }
}
