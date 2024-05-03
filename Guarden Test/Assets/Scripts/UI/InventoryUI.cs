using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.UI
{
    /// <summary>
    /// InventoryUI stores all of the player items
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField, Tooltip("ItemUI prefab")]
        private ItemUI itemPrefab;
        [SerializeField, Tooltip("Parent containing all itemsUIs")]
        private Transform itemParents;
        [SerializeField]
        private ObjectPool<ItemUI> pool;

        private List<ItemUI> items = new List<ItemUI>();

        /// <summary>
        /// Add item to the ui and items list
        /// </summary>
        /// <returns>Newly added item</returns>
        public ItemUI AddItem()
        {
            ItemUI itemUI = pool.GetPooledObject();
            itemUI.transform.SetParent(itemParents);
            itemUI.SetParent(this);
            items.Add(itemUI);
            return itemUI;
        }

        /// <summary>
        /// Remove item from ui and list once it's destroyed
        /// </summary>
        /// <param name="item">Destroyed item</param>
        internal void RemoveItem(ItemUI item)
        {
            items.Remove(item);
        }

        private void OnDisable()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                items[i].ReturnToPool();
            }
        }
    }
}
