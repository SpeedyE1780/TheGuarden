using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Reset Pools once game ends
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Pool Reseter")]
    internal class ObjectPoolReseter : ScriptableObject
    {
        internal delegate void ResetPool();
        internal static event ResetPool OnReset;

        /// <summary>
        /// Invoke reset pool event
        /// </summary>
        public void ResetPools()
        {
            OnReset.Invoke();
        }
    }
}
