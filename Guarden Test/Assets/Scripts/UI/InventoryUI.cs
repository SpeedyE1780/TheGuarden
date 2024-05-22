using System.Collections;
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
        [SerializeField, Tooltip("Parent containing all itemsUIs")]
        private Transform itemParents;
        [SerializeField, Tooltip("Pool from which itemUI are retrieved")]
        private ObjectPool<ItemUI> pool;
        [SerializeField, Tooltip("UI Showing selectedItem")]
        private ItemUI selectedItem;
        [SerializeField, Tooltip("Inventory selections window")]
        private GameObject inventorySelection;
        [SerializeField, Tooltip("Duration before hiding window")]
        private float hideDelay = 0.5f;

        private List<ItemUI> items = new List<ItemUI>();
        private float delay = 0.0f;

        public ItemUI SelectedViewer => selectedItem;

        /// <summary>
        /// Add item to the ui and items list
        /// </summary>
        /// <returns>Newly added item</returns>
        public ItemUI AddItem()
        {
            ItemUI itemUI = pool.GetPooledObject();
            itemUI.transform.SetParent(itemParents);
            itemUI.transform.localScale = Vector3.one;
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

        /// <summary>
        /// Clear inventory once player leaves game
        /// </summary>
        public void OnPlayerLeft()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                items[i].ReturnToPool();
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Reset delay to initial value
        /// </summary>
        public void ResetHideDelay()
        {
            delay = hideDelay;
            StartCoroutine(HideSelectionWindow());
        }


        /// <summary>
        /// Shows inventory selection UI then hides it and only shows the selected item
        /// </summary>
        /// <returns></returns>
        private IEnumerator HideSelectionWindow()
        {
            if (inventorySelection.activeSelf)
            {
                yield break;
            }

            inventorySelection.SetActive(true);
            selectedItem.gameObject.SetActive(false);

            while (delay > 0.0f)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            inventorySelection.SetActive(false);
            selectedItem.gameObject.SetActive(true);
        }
    }
}
