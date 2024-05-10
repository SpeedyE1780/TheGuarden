using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// A collection of disabled instantiated object to be reused
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public abstract class ObjectPool<T> : ScriptableObject where T : Object, IPoolObject
    {
        [SerializeField, Tooltip("Factory in charge of spawning new object if pool empty")]
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

        /// <summary>
        /// Clear pooledObjects list
        /// </summary>
        private void ResetPooledObjects()
        {
            pooledObjects.Clear();
        }

        /// <summary>
        /// Get a disabled object from the pool
        /// </summary>
        /// <returns>Object from pool</returns>
        private T GetObjectFromPool()
        {
            T pooledObject = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            pooledObject.OnExitPool();
            GameLogger.LogInfo($"Getting {pooledObject.name} from {name} pool", this, GameLogger.LogCategory.ObjectPooling);
            return pooledObject;
        }

        /// <summary>
        /// Create a new object with the factory
        /// </summary>
        /// <returns>Newly created object</returns>
        private T CreateNewObject()
        {
            GameLogger.LogInfo($"Create object from {factory.name} factory", this, GameLogger.LogCategory.ObjectPooling);
            return factory.CreateObject();
        }

        /// <summary>
        /// Get object from pool
        /// </summary>
        /// <returns>Object from pool</returns>
        public T GetPooledObject()
        {
            return pooledObjects.Count == 0 ? CreateNewObject() : GetObjectFromPool();
        }

        /// <summary>
        /// Add object to pool
        /// </summary>
        /// <param name="pooledObject">Object to be added to list</param>
        public void AddObject(T pooledObject)
        {
            pooledObject.OnEnterPool();
            pooledObjects.Add(pooledObject);
        }
    }
}
