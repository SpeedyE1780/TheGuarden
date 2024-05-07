using UnityEngine;

namespace TheGuarden.Utility
{
    public class SimpleFactory<T> : ObjectFactory<T> where T : Object
    {
        [SerializeField]
        private T prefab;

        internal override T CreateObject()
        {
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
