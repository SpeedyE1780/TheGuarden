using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TheGuarden.UI;
using TheGuarden.Utility;

namespace TheGuarden.Players
{
    /// <summary>
    /// PlayerInventory handles inventory and interaction with items
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField, Tooltip("InventoryUI in scene")]
        private InventoryUI inventoryUI;
        [SerializeField, Tooltip("Parent of all picked up items")]
        private Transform inventoryPoint;

        private List<IInventoryItem> items = new List<IInventoryItem>();
        private GameObject currentPickUp;
        private int selectedItemIndex = -1;
        private IInventoryItem selectedItem;

        /// <summary>
        /// Set and activate the player's inventory UI
        /// </summary>
        /// <param name="UI">The player's inventory UI</param>
        internal void SetInventoryUI(InventoryUI UI)
        {
            inventoryUI = UI;
            inventoryUI.gameObject.SetActive(true);
        }

        /// <summary>
        /// Called from the PlayerInput component
        /// </summary>
        /// <param name="context">Input context</param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started && selectedItem != null)
            {
                selectedItem.OnInteractionStarted();
            }

            if (context.performed && selectedItem != null)
            {
                selectedItem.OnInteractionPerformed();

                if (selectedItem.IsConsumedAfterInteraction)
                {
                    items.Remove(selectedItem);
                    selectedItemIndex = -1;
                    selectedItem = null;
                }
            }

            if (context.canceled && selectedItem != null)
            {
                selectedItem.OnInteractionCancelled();
            }
        }

        /// <summary>
        /// Called from the PlayerInput component
        /// </summary>
        /// <param name="context">Input context</param>
        public void OnPickUp(InputAction.CallbackContext context)
        {
            if (currentPickUp == null || context.canceled)
            {
                return;
            }

            GameLogger.LogInfo("STARTED/PERFORMED INTERACTION PICKUP", gameObject, GameLogger.LogCategory.Player);
            IPickUp pickUp = currentPickUp.GetComponent<IPickUp>();

            if ((context.started && pickUp.HasInstantPickUp) || context.performed)
            {
                PickUp(pickUp);
            }
        }

        /// <summary>
        /// Pick up and add item to inventory
        /// </summary>
        /// <param name="pickUp">Picked up item</param>
        private void PickUp(IPickUp pickUp)
        {
            pickUp.PickUp(inventoryPoint);
            currentPickUp = null;
            AddItemToInventory(pickUp.GetInventoryItem());
        }

        /// <summary>
        /// Add picked up inventory item to inventory and UI
        /// </summary>
        /// <param name="inventoryItem">Picked up inventory item</param>
        private void AddItemToInventory(IInventoryItem inventoryItem)
        {
            if (inventoryItem == null)
            {
                return;
            }

            items.Add(inventoryItem);
            inventoryItem.SetItemUI(inventoryUI.AddItem());
        }

        /// <summary>
        /// Called from PlayerInput component
        /// </summary>
        /// <param name="context"></param>
        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            selectedItem?.Deselect();

            if (selectedItemIndex + 1 >= items.Count)
            {
                selectedItemIndex = -1;
                selectedItem = null;
            }
            else
            {
                selectedItemIndex += 1;
                selectedItem = items[selectedItemIndex];
                selectedItem.Select();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Tags.HasTag(other.gameObject, Tags.Plant, Tags.Bucket) && currentPickUp == null)
            {
                currentPickUp = other.gameObject;
                GameLogger.LogInfo("ENTER PLANT", gameObject, GameLogger.LogCategory.Player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == currentPickUp)
            {
                currentPickUp = null;
                GameLogger.LogInfo("EXIT PLANT", gameObject, GameLogger.LogCategory.Player);
            }
        }
    }
}
