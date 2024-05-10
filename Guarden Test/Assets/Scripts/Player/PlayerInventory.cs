using System.Collections.Generic;
using TheGuarden.Interactable;
using TheGuarden.UI;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheGuarden.Players
{
    /// <summary>
    /// PlayerInventory handles inventory and interaction with items
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField, Tooltip("Maximum items in inventory")]
        private int inventorySize = 5;
        [SerializeField, Tooltip("Parent of all picked up items")]
        private Transform inventoryPoint;
        [SerializeField, Tooltip("Instructions game event raised when interactio instructions are active")]
        private TGameEvent<string> onInstructions;
        [SerializeField, Tooltip("Hide instructions game event")]
        private GameEvent onHideInstructions;
        [SerializeField, Tooltip("Player input component")]
        private PlayerInput playerInput;
        [SerializeField, Tooltip("Pressing to pick up instructions")]
        private InteractionInstruction pressPickUpInstruction;
        [SerializeField, Tooltip("Hodling to pick up instructions")]
        private InteractionInstruction holdPickUpInstruction;
        [SerializeField, Tooltip("Player bucket added to inventory on start")]
        private Bucket bucket;

        private InventoryUI inventoryUI;
        private List<IInventoryItem> items;
        private IPickUp currentPickUp;
        private int selectedItemIndex = -1;
        private IInventoryItem selectedItem;
        private bool showPickUpInstruction = false;

        private void Start()
        {
            items = new List<IInventoryItem>(inventorySize);
            bucket = Instantiate(bucket);
            PickUp(bucket);
        }

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
        /// Set selected item to first item or null
        /// </summary>
        private void UpdateSelectedItem()
        {
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
                    UpdateSelectedItem();
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
            if (currentPickUp == null || context.canceled || items.Count == inventorySize)
            {
                return;
            }

            GameLogger.LogInfo("STARTED/PERFORMED INTERACTION PICKUP", gameObject, GameLogger.LogCategory.Player);

            if ((context.started && currentPickUp.HasInstantPickUp) || context.performed)
            {
                PickUp(currentPickUp);
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

        /// <summary>
        /// Empty player inventory before dropping out
        /// </summary>
        internal void EmptyInventory()
        {
            foreach (IInventoryItem item in items)
            {
                item.Drop();
            }

            items.Clear();
            inventoryUI.OnPlayerLeft();
        }

        public void OnDropItem(InputAction.CallbackContext context)
        {
            if (selectedItem == null || !context.performed)
            {
                return;
            }

            selectedItem.Drop();
            items.Remove(selectedItem);
            UpdateSelectedItem();
        }

        private void Update()
        {
            if (selectedItem != null)
            {
                InteractionInstruction instruction = selectedItem.CheckForInteractable();

                if (instruction != null)
                {
                    onInstructions.Raise(instruction.GetInstructionMessage(playerInput.currentControlScheme));
                }
                else if (!showPickUpInstruction)
                {
                    onHideInstructions.Raise();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.PickUp))
            {
                if ((currentPickUp == null || !currentPickUp.gameObject.activeSelf))
                {
                    currentPickUp = other.attachedRigidbody.GetComponent<IPickUp>();
                    GameLogger.LogInfo("ENTER PICK UP", gameObject, GameLogger.LogCategory.Player);
                    showPickUpInstruction = true;
                }

                string pickUpInstruction = currentPickUp.HasInstantPickUp ? pressPickUpInstruction.GetInstructionMessage(playerInput.currentControlScheme) : holdPickUpInstruction.GetInstructionMessage(playerInput.currentControlScheme);
                onInstructions.Raise(pickUpInstruction);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (currentPickUp != null && other.attachedRigidbody != null && other.attachedRigidbody.gameObject == currentPickUp.gameObject)
            {
                currentPickUp = null;
                GameLogger.LogInfo("EXIT PICK UP", gameObject, GameLogger.LogCategory.Player);
                showPickUpInstruction = false;
            }

            onHideInstructions.Raise();
        }
    }
}
