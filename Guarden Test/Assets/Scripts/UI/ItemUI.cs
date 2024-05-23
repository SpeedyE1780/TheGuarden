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
        [SerializeField, Tooltip("Black & White image")]
        private Image bwImage;
        [SerializeField, Tooltip("Colored image")]
        private Image coloredImage;
        [SerializeField, Tooltip("Pool this item is returned to")]
        private ObjectPool<ItemUI> pool;
        [SerializeField, Tooltip("Sprite When Selected")]
        private Sprite selectedSprite;
        [SerializeField, Tooltip("Sprite When Not Selected")]
        private Sprite notSelectedSprite;
        [SerializeField,Tooltip("Sprite Component")] 
        private Image spriteComponent;

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
        public void SetItem(string itemName, float progress, ItemIconPair icons)
        {
            nameText.text = itemName;
            bwImage.enabled = icons.bwIcon != null;
            bwImage.sprite = icons.bwIcon;

            coloredImage.enabled = icons.coloredIcon != null;
            coloredImage.sprite = icons.coloredIcon;
            coloredImage.fillAmount = progress;
        }

        /// <summary>
        /// Set item on screen info based on reference
        /// </summary>
        /// <param name="reference">Item who's info is shown</param>
        private void SetItem(ItemUI reference)
        {
            nameText.color = Color.yellow;
            nameText.text = reference.nameText.text;

            bwImage.enabled = reference.bwImage.enabled;
            bwImage.sprite = reference.bwImage.sprite;

            coloredImage.enabled = reference.coloredImage.enabled;
            coloredImage.sprite = reference.coloredImage.sprite;
            coloredImage.fillAmount = reference.coloredImage.fillAmount;

            spriteComponent.sprite = selectedSprite;
        }

        private void ClearItem()
        {
            nameText.color = Color.white;
            nameText.text = string.Empty;
            coloredImage.fillAmount = 0;
            bwImage.enabled = false;
            bwImage.sprite = null;
            coloredImage.enabled = false;
            coloredImage.sprite = null;

        }

        /// <summary>
        /// Set progress slider value
        /// </summary>
        /// <param name="progress">Progress slider value</param>
        public void SetProgress(float progress)
        {
            coloredImage.fillAmount = progress;

            if (viewer != null)
            {
                viewer.coloredImage.fillAmount = progress;
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

            spriteComponent.sprite = selectedSprite;
        }

        /// <summary>
        /// Stop highlighting element on screen
        /// </summary>
        public void Deselect()
        {
            nameText.color = Color.white;
            viewer.ClearItem();
            viewer = null;

            spriteComponent.sprite = notSelectedSprite;
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
            coloredImage.fillAmount = 0;
            nameText.text = "";
            nameText.color = Color.white;

            spriteComponent.sprite = notSelectedSprite;
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
