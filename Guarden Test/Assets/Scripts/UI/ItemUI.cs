using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TheGuarden.Utility;

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
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private ObjectPool<ItemUI> pool;

        private InventoryUI inventoryUI;

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
        /// Set progress slider value
        /// </summary>
        /// <param name="progress">Progress slider value</param>
        public void SetProgress(float progress)
        {
            progressSlider.value = progress;
        }

        /// <summary>
        /// Highlight element on screen
        /// </summary>
        public void Select()
        {
            nameText.color = Color.yellow;
        }

        /// <summary>
        /// Stop highlighting element on screen
        /// </summary>
        public void Deselect()
        {
            nameText.color = Color.white;
        }

        public void ReturnToPool()
        {
            pool.AddObject(this);
        }

        public void OnEnterPool()
        {
            inventoryUI.RemoveItem(this);
            gameObject.SetActive(false);
            progressSlider.value = 0;
            nameText.text = "";
            nameText.color = Color.white;
        }

        public void OnExitPool()
        {
            gameObject.SetActive(true);
            transform.SetParent(null);
        }
    }
}
