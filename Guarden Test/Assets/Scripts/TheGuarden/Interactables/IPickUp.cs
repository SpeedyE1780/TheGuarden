using UnityEngine;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// IPickUp represents an item that can be picked up
    /// </summary>
    public interface IPickUp
    {
        public bool HasInstantPickUp { get; }

        /// <summary>
        /// Pick up item from scene
        /// </summary>
        /// <param name="parent">New item parent</param>
        public void PickUp(Transform parent);

        /// <summary>
        /// Get inventory item received from picking up
        /// </summary>
        /// <returns>Inventory item received from picking up</returns>
        public IInventoryItem GetInventoryItem();
    }
}
