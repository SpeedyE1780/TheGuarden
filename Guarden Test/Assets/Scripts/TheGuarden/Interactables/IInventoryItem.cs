using TheGuarden.UI;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// IInventory represents an item that's in the player inventory and can be used
    /// </summary>
    public interface IInventoryItem
    {
        public string Name { get; }
        public float UsabilityPercentage { get; }
        public ItemUI ItemUI { get; set; }
        public bool IsConsumedAfterInteraction => false;

        /// <summary>
        /// Highlight item in inventory
        /// </summary>
        public void Select()
        {
            if (ItemUI != null)
            {
                ItemUI.Select();
            }
        }

        /// <summary>
        /// Stop highlighting item in inventory
        /// </summary>
        public void Deselect()
        {
            if (ItemUI != null)
            {
                ItemUI.Deselect();
            }
        }

        /// <summary>
        /// Set UI representing this item
        /// </summary>
        /// <param name="itemUI">ItemUI representing this item</param>
        public void SetItemUI(ItemUI itemUI)
        {
            ItemUI = itemUI;
            itemUI.SetItem(Name, UsabilityPercentage);
        }

        /// <summary>
        /// Called when interaction with item is started
        /// </summary>
        public void OnInteractionStarted();

        /// <summary>
        /// Called when interaction with item is performed
        /// </summary>
        public void OnInteractionPerformed();

        /// <summary>
        /// Called when interaction with item is cancelled
        /// </summary>
        public void OnInteractionCancelled();
    } 
}
