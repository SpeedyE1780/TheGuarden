using UnityEngine;

namespace TheGuarden.Utility
{
    public abstract class ObjectFactory<T> : ScriptableObject where T : Object
    {
        internal abstract T CreateObject();

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

        protected void RenameObject(T obj, string prefabName)
        {
            obj.name = prefabName + " " + count;
            count += 1;

        }
#endif
    }
}
