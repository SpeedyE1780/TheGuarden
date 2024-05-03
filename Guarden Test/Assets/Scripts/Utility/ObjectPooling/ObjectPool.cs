using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public abstract class ObjectPool : ScriptableObject
    {
    }

    public abstract class ObjectPool<T> : ObjectPool
    {
        [SerializeField]
        private ObjectFactory<T> factory;

        private List<T> pooledObjects = new List<T>();

        private T GetObjectFromPool()
        {
            T pooledObject = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            return pooledObject;
        }

        private T CreateNewObject()
        {
            return factory.CreateObject();
        }

        public T GetPooledObject()
        {
            return pooledObjects.Count == 0 ? CreateNewObject() : GetObjectFromPool();
        }

        public void AddObject(T obj)
        {
            pooledObjects.Add(obj);
        }
    }
}
