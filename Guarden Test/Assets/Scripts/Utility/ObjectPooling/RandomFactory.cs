using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Choose a random prefab from list and instantiate it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RandomFactory<T> : ObjectFactory<T> where T : Object
    {
        [SerializeField, Tooltip("List of possible prefab that will be instantiated")]
        private List<T> prefabs;

        /// <summary>
        /// Instantiate Object in scene
        /// </summary>
        /// <returns>New object</returns>
        internal override T CreateObject()
        {
            T prefab = prefabs.GetRandomItem();

            GameLogger.LogInfo($"Factory {name} creating {prefab.name}", this, GameLogger.LogCategory.ObjectPooling);

#if UNITY_EDITOR
            T obj = Instantiate(prefab);
            RenameObject(obj, prefab.name);
            return obj;
#else
            return Instantiate(prefab);
#endif
        }
    }
}
