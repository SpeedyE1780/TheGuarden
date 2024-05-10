using UnityEngine;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Pool Reseter")]
    internal class ObjectPoolReseter : ScriptableObject
    {
        internal delegate void ResetPool();
        internal static event ResetPool OnReset;

        public void ResetPools()
        {
            OnReset?.Invoke();
        }
    }
}
