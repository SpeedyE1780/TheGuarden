using UnityEngine;

namespace TheGuarden.Utility
{
    public class SimpleFactory<T> : ObjectFactory<T> where T : Object
    {
        [SerializeField]
        private T prefab;

#if UNITY_EDITOR
        internal int count = 0;

        private void OnEnable()
        {
            ObjectPoolReseter.OnReset += ResetCount;
        }

        private void OnDisable()
        {
            ObjectPoolReseter.OnReset -= ResetCount;
        }

        private void ResetCount()
        {
            count = 0;
        }
#endif

        internal override T CreateObject()
        {
            GameLogger.LogInfo($"Factory {name} creating {prefab.name}", this, GameLogger.LogCategory.ObjectPooling);

#if UNITY_EDITOR
            T obj = Instantiate(prefab);
            obj.name = prefab.name + " " + count;
            count += 1;
            return obj;
#else
            return Instantiate(prefab);
#endif
        }
    }
}
