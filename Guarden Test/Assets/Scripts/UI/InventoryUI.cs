using System.Collections.Generic;
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

        private List<ItemUI> items = new List<ItemUI>();

        /// <summary>
        /// Add item to the ui and items list
        /// </summary>
        /// <returns>Newly added item</returns>
        public ItemUI AddItem()
        {
            ItemUI itemUI = Instantiate(itemPrefab, itemParents);
            itemUI.SetParent(this);
            items.Add(itemUI);
            return itemUI;
        }

        /// <summary>
        /// Select item based on index
        /// </summary>
        /// <param name="index">Item index</param>
        public void SelectItem(int index)
        {
            items[index].Select();
        }

        /// <summary>
        /// Deselect item based on index
        /// </summary>
        /// <param name="index">Item index</param>
        public void DeselectItem(int index)
        {
            items[index].Deselect();
        }

        /// <summary>
        /// Remove item from ui and list once it's destroyed
        /// </summary>
        /// <param name="item">Destroyed item</param>
        internal void RemoveItem(ItemUI item)
        {
            items.Remove(item);
        }
    }
}
