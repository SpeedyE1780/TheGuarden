using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public class RandomFactory<T> : ObjectFactory<T> where T : Object
    {
        [SerializeField]
        private List<T> prefabs;

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
