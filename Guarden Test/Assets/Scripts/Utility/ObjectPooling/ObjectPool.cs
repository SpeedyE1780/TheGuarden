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
            T pooledObject = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            pooledObject.OnExitPool();
            GameLogger.LogInfo($"Getting {pooledObject.name} from {name} pool", this, GameLogger.LogCategory.ObjectPooling);
            return pooledObject;
        }

        private T CreateNewObject()
        {
            GameLogger.LogInfo($"Create object from {factory.name} factory", this, GameLogger.LogCategory.ObjectPooling);
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
