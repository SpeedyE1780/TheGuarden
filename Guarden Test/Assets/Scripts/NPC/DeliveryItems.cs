using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// List of items that truck should spawn
    /// </summary>
    internal class DeliveryItems<Item> : ScriptableObject where Item : Object, IPoolObject
    {
        [SerializeField, Tooltip("Guaranteed items that will be delivered")]
        private List<DeliveryItem<Item>> guaranteed = new List<DeliveryItem<Item>>();
        [SerializeField, Tooltip("Random items that might be delivered")]
        private List<DeliveryItem<Item>> random = new List<DeliveryItem<Item>>();
        [SerializeField, Tooltip("Random item count")]
        internal int count = 0;

        private List<ObjectPool<Item>> unlockedGuaranteed = new List<ObjectPool<Item>>();
        private List<ObjectPool<Item>> unlockedRandom = new List<ObjectPool<Item>>();

        internal List<ObjectPool<Item>> Guaranteed => unlockedGuaranteed;
        internal List<ObjectPool<Item>> Random => unlockedRandom;

        /// <summary>
        /// Add item to unlocked list if unlocked
        /// </summary>
        /// <param name="deliveryItem">Item that's being added</param>
        /// <param name="unlocked">List of unlocked items</param>
        /// <returns>True if item was added</returns>
        private bool TryAddItem(DeliveryItem<Item> deliveryItem, List<ObjectPool<Item>> unlocked)
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
        private void AddUnlockedItems(List<DeliveryItem<Item>> source, List<ObjectPool<Item>> unlocked)
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
        private void CloneDeliveryItems(List<DeliveryItem<Item>> source, List<DeliveryItem<Item>> destination, List<ObjectPool<Item>> unlocked)
        {
            foreach (DeliveryItem<Item> item in source)
            {
                DeliveryItem<Item> clone = Instantiate(item);

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

        private void ClearLists()
        {
            guaranteed.Clear();
            random.Clear();
        }

        /// <summary>
        /// Clone current instance
        /// </summary>
        /// <returns>A deep copy clone of this class</returns>
        internal DeliveryItems<Item> Clone()
        {
            DeliveryItems<Item> clone = Instantiate(this);
            clone.ClearLists();
            clone.count = count;
            clone.CloneDeliveryItems(guaranteed, clone.guaranteed, clone.unlockedGuaranteed);
            clone.CloneDeliveryItems(random, clone.random, clone.unlockedRandom);
            return clone;
        }
    }
}
