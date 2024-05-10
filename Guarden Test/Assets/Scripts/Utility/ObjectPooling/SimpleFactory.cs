using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Simple factory has one prefab and always spawns it
    /// </summary>
    /// <typeparam name="T">Factory Item that will be spawned</typeparam>
    public class SimpleFactory<T> : ObjectFactory<T> where T : Object
    {
        [SerializeField, Tooltip("Prefab to spawn")]
        private T prefab;

        /// <summary>
        /// Instantiate Object in scene
        /// </summary>
        /// <returns>New object</returns>
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
