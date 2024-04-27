using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.NPC
{
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

        private bool TryAddItem(DeliveryItem deliveryItem, List<GameObject> unlocked)
        {
            deliveryItem.OnDayEnded();

            if (deliveryItem.IsUnlocked)
            {
                GameLogger.LogInfo($"{deliveryItem.name} Unlocked", this, GameLogger.LogCategory.PlantPowerUp);
                unlocked.Add(deliveryItem.item);
                return true;
            }

            return false;
        }

        internal void OnDayEnded()
        {
            for (int i = guaranteed.Count - 1; i >= 0; --i)
            {
                if (TryAddItem(guaranteed[i], unlockedGuaranteed))
                {
                    guaranteed.RemoveAt(i);
                }
            }

            for (int i = random.Count - 1; i >= 0; --i)
            {
                if (TryAddItem(random[i], unlockedRandom))
                {
                    random.RemoveAt(i);
                }
            }
        }

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
