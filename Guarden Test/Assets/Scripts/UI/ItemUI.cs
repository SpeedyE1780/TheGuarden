using TheGuarden.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    /// <summary>
    /// ItemUI represents one item in the player's inventory on the screen
    /// </summary>
    public class ItemUI : MonoBehaviour, IPoolObject
    {
        [SerializeField, Tooltip("Item name text")]
        private TMP_Text nameText;
        [SerializeField, Tooltip("Item progress slider")]
        private Slider progressSlider;
        [SerializeField, Tooltip("Item image")]
        private Image itemImage;
        [SerializeField, Tooltip("Pool this item is returned to")]
        private ObjectPool<ItemUI> pool;

        private InventoryUI inventoryUI;
        private ItemUI viewer;

        /// <summary>
        /// Set InventoryUI containing this item
        /// </summary>
        /// <param name="inventory">InventoryUI containing this item</param>
        internal void SetParent(InventoryUI inventory)
        {
            inventoryUI = inventory;
        }

        /// <summary>
        /// Set item on screen info
        /// </summary>
        /// <param name="itemName">Item Name</param>
        /// <param name="progress">Item progress</param>
        public void SetItem(string itemName, float progress, Sprite icon)
        {
            nameText.text = itemName;
            progressSlider.value = progress;
            itemImage.enabled = icon != null;
            itemImage.sprite = icon;
        }

        /// <summary>
        /// Set item on screen info based on reference
        /// </summary>
        /// <param name="reference">Item who's info is shown</param>
        private void SetItem(ItemUI reference)
        {
            nameText.text = reference.nameText.text;
            progressSlider.value = reference.progressSlider.value;
            itemImage.enabled = reference.itemImage.enabled;
            itemImage.sprite = reference.itemImage.sprite;
        }

        private void ClearItem()
        {
            nameText.text = string.Empty;
            progressSlider.value = 0;
            itemImage.enabled = false;
            itemImage.sprite = null;
        }

        /// <summary>
        /// Set progress slider value
        /// </summary>
        /// <param name="progress">Progress slider value</param>
        public void SetProgress(float progress)
        {
            progressSlider.value = progress;

            if (viewer != null)
            {
                viewer.progressSlider.value = progress;
            }
        }

        /// <summary>
        /// Highlight element on screen
        /// </summary>
        public void Select()
        {
            nameText.color = Color.yellow;
            viewer = inventoryUI.SelectedViewer;
            viewer.SetItem(this);
        }

        /// <summary>
        /// Stop highlighting element on screen
        /// </summary>
        public void Deselect()
        {
            nameText.color = Color.white;
            viewer.ClearItem();
            viewer = null;
        }

        /// <summary>
        /// Return this item to pool
        /// </summary>
        public void ReturnToPool()
        {
            pool.AddObject(this);
        }

        /// <summary>
        /// Reset state befor entering pool
        /// </summary>
        public void OnEnterPool()
        {
            inventoryUI.RemoveItem(this);
            gameObject.SetActive(false);
            progressSlider.value = 0;
            nameText.text = "";
            nameText.color = Color.white;
        }

        /// <summary>
        /// Reset state when exiting pool
        /// </summary>
        public void OnExitPool()
        {
            gameObject.SetActive(true);
            transform.SetParent(null);
        }
    }
}
