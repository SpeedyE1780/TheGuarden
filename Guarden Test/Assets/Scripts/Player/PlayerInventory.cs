using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TheGuarden.Interactable;
using TheGuarden.UI;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;

namespace TheGuarden.Players
{
    /// <summary>
    /// PlayerInventory handles inventory and interaction with items
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField, Tooltip("Parent of all picked up items")]
        private Transform inventoryPoint;
        [SerializeField]
        private TGameEvent<string> onInstructions;
        [SerializeField]
        private GameEvent onHideInstructions;

        private InventoryUI inventoryUI;
        private List<IInventoryItem> items = new List<IInventoryItem>();
        private GameObject currentPickUp;
        private int selectedItemIndex = -1;
        private IInventoryItem selectedItem;
        private bool showPickUpInstruction = false;

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

                    if (items.Count == 0)
                    {
                        selectedItemIndex = -1;
                        selectedItem = null;
                    }
                    else
                    {
                        SelectItem(0);
                    }
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
            showPickUpInstruction = false;
            onHideInstructions.Raise();
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

            if (items.Count == 1)
            {
                SelectItem(0);
            }
        }

        /// <summary>
        /// Select item at index in inventory
        /// </summary>
        /// <param name="index">Item index in inventory</param>
        private void SelectItem(int index)
        {
            selectedItemIndex = index;
            selectedItem = items[index];
            selectedItem.Select();
        }

        /// <summary>
        /// Called from PlayerInput component
        /// </summary>
        /// <param name="context"></param>
        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (!context.performed || items.Count == 0)
            {
                return;
            }

            selectedItem?.Deselect();
            int newItemIndex = selectedItemIndex + 1 >= items.Count ? 0 : selectedItemIndex + 1;
            SelectItem(newItemIndex);
        }

        /// <summary>
        /// Called from PlayerInput component
        /// </summary>
        /// <param name="context"></param>
        public void OnPreviousItem(InputAction.CallbackContext context)
        {
            if (!context.performed || items.Count == 0)
            {
                return;
            }

            selectedItem?.Deselect();
            int newIndex = selectedItemIndex - 1 < 0 ? items.Count - 1 : selectedItemIndex - 1;
            SelectItem(newIndex);
        }

        public void EmptyInventory()
        {
            foreach (IInventoryItem item in items)
            {
                item.Drop();
            }

            items.Clear();
            inventoryUI.OnPlayerLeft();
        }

        private void Update()
        {
            if (selectedItem != null)
            {
                if (!selectedItem.CheckForInteractable() && !showPickUpInstruction)
                {
                    onHideInstructions.Raise();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (currentPickUp == null && other.CompareTag(Tags.PickUp))
            {
                currentPickUp = other.attachedRigidbody.gameObject;
                GameLogger.LogInfo("ENTER PICK UP", gameObject, GameLogger.LogCategory.Player);
                onInstructions.Raise("Press/Hold space to pick up item");
                showPickUpInstruction = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody != null && other.attachedRigidbody.gameObject == currentPickUp)
            {
                currentPickUp = null;
                GameLogger.LogInfo("EXIT PICK UP", gameObject, GameLogger.LogCategory.Player);
                showPickUpInstruction = false;
            }

            onHideInstructions.Raise();
        }
    }
}
