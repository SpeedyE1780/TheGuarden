using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    /// <summary>
    /// ItemUI represents one item in the player's inventory on the screen
    /// </summary>
    public class ItemUI : MonoBehaviour
    {
        [SerializeField, Tooltip("Item name text")]
        private TMP_Text nameText;
        [SerializeField, Tooltip("Item progress slider")]
        private Slider progressSlider;

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
        public void SetItem(string itemName, float progress)
        {
            nameText.text = itemName;
            progressSlider.value = progress;
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

        private void OnDestroy()
        {
            if (inventoryUI != null)
            {
                inventoryUI.RemoveItem(this);
            }
        }
    }
}
