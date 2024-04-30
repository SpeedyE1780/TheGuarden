using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// DeliveryItem wraps gameobject to spawn and determine when its unlocked
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Deliveries/Item")]
    internal class DeliveryItem : ScriptableObject
    {
        [SerializeField, Tooltip("GameObject to spawn")]
        internal GameObject item;
        [SerializeField, Tooltip("Days before item is unlocked")]
        private int daysToUnlock = 0;

        internal bool IsUnlocked => daysToUnlock <= 0;
        private int lastFrame = 0;

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
