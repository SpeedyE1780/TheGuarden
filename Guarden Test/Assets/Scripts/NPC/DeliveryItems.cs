using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// List of items that truck should spawn
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Deliveries/Items List")]
    internal class DeliveryItems : ScriptableObject
    {
        [SerializeField, Tooltip("Guaranteed items that will be delivered")]
        private List<DeliveryItem> guaranteed = new List<DeliveryItem>();
        [SerializeField, Tooltip("Random items that might be delivered")]
        private List<DeliveryItem> random = new List<DeliveryItem>();
        [SerializeField, Tooltip("Random item count")]
        internal int count = 0;

        private List<GameObject> unlockedGuaranteed = new List<GameObject>();
        private List<GameObject> unlockedRandom = new List<GameObject>();

        internal List<GameObject> Guaranteed => unlockedGuaranteed;
        internal List<GameObject> Random => unlockedRandom;

        /// <summary>
        /// Add item to unlocked list if unlocked
        /// </summary>
        /// <param name="deliveryItem">Item that's being added</param>
        /// <param name="unlocked">List of unlocked items</param>
        /// <returns>True if item was added</returns>
        private bool TryAddItem(DeliveryItem deliveryItem, List<GameObject> unlocked)
        {
            deliveryItem.OnDayStarted();

            if (deliveryItem.IsUnlocked)
            {
                GameLogger.LogInfo($"{deliveryItem.name} Unlocked", this, GameLogger.LogCategory.PlantPowerUp);
                unlocked.Add(deliveryItem.item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add unlocked items to list
        /// </summary>
        /// <param name="source">Source of items</param>
        /// <param name="unlocked">List of unlocked items</param>
        private void AddUnlockedItems(List<DeliveryItem> source, List<GameObject> unlocked)
        {
            for (int i = source.Count - 1; i >= 0; --i)
            {
                if (TryAddItem(source[i], unlocked))
                {
                    source.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Called when day ends and unlock items
        /// </summary>
        internal void OnDayStarted()
        {
            AddUnlockedItems(guaranteed, unlockedGuaranteed);
            AddUnlockedItems(random, unlockedRandom);
        }

        /// <summary>
        /// Clone delivery items and add them to destination/unlocked
        /// </summary>
        /// <param name="source">Source of items to clone</param>
        /// <param name="destination">Destination where items should be added if not unlocked</param>
        /// <param name="unlocked">List of unlocked items</param>
        private void CloneDeliveryItems(List<DeliveryItem> source, List<DeliveryItem> destination, List<GameObject> unlocked)
        {
            foreach (DeliveryItem item in source)
            {
                DeliveryItem clone = Instantiate(item);

                if (clone.IsUnlocked)
                {
                    GameLogger.LogInfo($"{item.name} is unlocked on start", this, GameLogger.LogCategory.PlantPowerUp);
                    unlocked.Add(item.item);
                }
                else
                {
                    destination.Add(clone);
                }
            }
        }

        /// <summary>
        /// Clone current instance
        /// </summary>
        /// <returns>A deep copy clone of this class</returns>
        internal DeliveryItems Clone()
        {
            DeliveryItems clone = CreateInstance<DeliveryItems>();
            clone.count = count;
            clone.CloneDeliveryItems(guaranteed, clone.guaranteed, clone.unlockedGuaranteed);
            clone.CloneDeliveryItems(random, clone.random, clone.unlockedRandom);
            return clone;
        }
    }
}
