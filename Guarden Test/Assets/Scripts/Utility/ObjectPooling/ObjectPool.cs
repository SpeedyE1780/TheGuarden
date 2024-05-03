using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public abstract class ObjectPool<T> : ScriptableObject where T : Object, IPoolObject
    {
        [SerializeField]
        private ObjectFactory<T> factory;

        private List<T> pooledObjects = new List<T>();

        private void OnEnable()
        {
            ObjectPoolReseter.OnReset += ResetPooledObjects;
        }


        private void OnDisable()
        {
            ObjectPoolReseter.OnReset -= ResetPooledObjects;
        }

        private void ResetPooledObjects()
        {
            pooledObjects.Clear();
        }

        private T GetObjectFromPool()
        {
            GameLogger.LogInfo("Getting object from pool", this, GameLogger.LogCategory.Scene);
            T pooledObject = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            pooledObject.OnExitPool();
            return pooledObject;
        }

        private T CreateNewObject()
        {
            GameLogger.LogInfo("Create object from factory", this, GameLogger.LogCategory.Scene);
            return factory.CreateObject();
        }

        public T GetPooledObject()
        {
            return pooledObjects.Count == 0 ? CreateNewObject() : GetObjectFromPool();
        }

        public void AddObject(T pooledObject)
        {
            pooledObject.OnEnterPool();
            pooledObjects.Add(pooledObject);
        }
    }
}
