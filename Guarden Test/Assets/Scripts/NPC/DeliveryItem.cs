using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// DeliveryItem wraps gameobject to spawn and determine when its unlocked
    /// </summary>
    internal abstract class DeliveryItem<T> : ScriptableObject where T : Object, IPoolObject
    {
        [SerializeField, Tooltip("GameObject to spawn")]
        internal ObjectPool<T> item;
        [SerializeField, Tooltip("Days before item is unlocked")]
        private int daysToUnlock = 0;

        internal bool IsUnlocked => daysToUnlock <= 0;
        private int lastFrame = 0;

        /// <summary>
        /// Called when item is unlocked
        /// </summary>
        public abstract void OnUnlocked();

        /// <summary>
        /// Decrement daysToUnlock
        /// </summary>
        internal void OnDayStarted()
        {
            //Prevent decrementing twice in same frame
            if (Time.frameCount == lastFrame)
            {
                return;
            }

            lastFrame = Time.frameCount;
            daysToUnlock -= 1;
        }
    }
}
