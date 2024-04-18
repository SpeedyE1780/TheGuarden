using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TheGuarden.UI;
using TheGuarden.Utility;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private Transform inventoryPoint;

    private List<IInventoryItem> items = new List<IInventoryItem>();
    private GameObject currentPickUp;
    private int selectedItemIndex;

    private void Start()
    {
        selectedItemIndex = -1;
    }

    public void SetInventoryUI(InventoryUI UI)
    {
        inventoryUI = UI;
        inventoryUI.gameObject.SetActive(true);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        IInventoryItem selectedItem = selectedItemIndex >= 0 && selectedItemIndex < items.Count ? items[selectedItemIndex] : null;

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
            }
        }

        if (context.canceled && selectedItem != null)
        {
            selectedItem.OnInteractionCancelled();
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (currentPickUp == null)
        {
            return;
        }

        if (context.started)
        {
            GameLogger.LogInfo("STARTED INTERACTION PICKUP", gameObject, GameLogger.LogCategory.Player);

            IPickUp pickUp = currentPickUp.GetComponent<IPickUp>();

            if (pickUp.HasInstantPickUp)
            {
                PickUp(pickUp);
            }
        }

        if (context.performed)
        {
            GameLogger.LogInfo("PERFORMED PICKUP", gameObject, GameLogger.LogCategory.Player);

            PickUp(currentPickUp.GetComponent<IPickUp>());
        }
    }

    private void PickUp(IPickUp pickUp)
    {
        pickUp.PickUp(inventoryPoint);
        currentPickUp = null;
        AddItemToInventory(pickUp.GetInventoryItem());
    }

    private void AddItemToInventory(IInventoryItem inventoryItem)
    {
        if (inventoryItem == null)
        {
            return;
        }

        items.Add(inventoryItem);
        inventoryItem.SetItemUI(inventoryUI.AddItem());
    }

    public void OnNextItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (selectedItemIndex + 1 >= items.Count)
            {
                if (selectedItemIndex != -1 && selectedItemIndex < items.Count)
                {
                    inventoryUI.DeselectItem(selectedItemIndex);
                }

                selectedItemIndex = -1;
            }
            else
            {
                if (selectedItemIndex != -1 && selectedItemIndex < items.Count)
                {
                    inventoryUI.DeselectItem(selectedItemIndex);
                }

                selectedItemIndex += 1;
                inventoryUI.SelectItem(selectedItemIndex);
            }
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
