using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private PlantingIndicator plantingIndicator;
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private LayerMask plantBedMask;

    private List<IInteractable> items = new List<IInteractable>();
    private GameObject currentInteractable;
    private GameObject currentSoil;
    private IInteractable selectedItem;


    private void Start()
    {
        inventoryUI.PlayerInventory = this;
        selectedItem = null;
        plantingIndicator.Mask = plantBedMask;
    }

    public void SetSelectedItem(int index)
    {
        selectedItem = items[index];
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);

            if (inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.FillUI(items);
            }
        }
    }

    private void OnInteractStarted()
    {
        Mushroom mushroom = selectedItem as Mushroom;
        plantingIndicator.gameObject.SetActive(true);
        plantingIndicator.UpdateMesh(mushroom.Mesh, mushroom.Materials);

        if (!mushroom.IsFullyGrown && currentSoil != null)
        {
            plantingIndicator.transform.position = currentSoil.transform.position;
            plantingIndicator.PlantingInSoil = true;
        }
    }

    private void OnInteractPerformed()
    {
        plantingIndicator.gameObject.SetActive(false);

        if (selectedItem != null)
        {
            bool planted = false;
            Mushroom mushroom = selectedItem as Mushroom;

            if (mushroom.IsFullyGrown)
            {
                Debug.Log("Plant anywhere");

                if (Physics.CheckSphere(plantingIndicator.transform.position, 2.0f, plantBedMask))
                {
                    Debug.Log("Can't plant in planting bed");
                    return;
                }

                mushroom.Plant(plantingIndicator.transform.position, plantingIndicator.transform.rotation);
                planted = true;
            }
            else if (currentSoil != null)
            {
                Debug.Log("Plant in soil");
                mushroom.PlantInSoil(currentSoil.transform.position, currentSoil.transform.rotation);
                planted = true;
            }

            if (planted)
            {
                items.Remove(mushroom);
                selectedItem = null;
                inventoryUI.HideSelected();
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && selectedItem != null)
        {
            OnInteractStarted();
        }

        if (context.performed)
        {
            OnInteractPerformed();
        }

        if (context.canceled)
        {
            plantingIndicator.gameObject.SetActive(false);
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.started && currentInteractable != null)
        {
            Debug.Log("STARTED INTERACTION PICKUP");

            IInteractable interactable = currentInteractable.GetComponent<IInteractable>();

            if (interactable.GrowthPercentage == 0)
            {
                items.Add(interactable);
                interactable.PickUp();
                currentInteractable = null;
            }
        }

        if (context.performed && currentInteractable != null)
        {
            Debug.Log("PERFORMED PICKUP");

            IInteractable interactable = currentInteractable.GetComponent<IInteractable>();
            items.Add(interactable);
            interactable.PickUp();
            currentInteractable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plant") && currentInteractable == null)
        {
            currentInteractable = other.gameObject;
            Debug.Log("ENTER PLANT");
        }

        if (other.CompareTag("PlantSoil") && currentSoil == null)
        {
            currentSoil = other.gameObject;
            Debug.Log("ENTER SOIL");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("EXIT PLANT");
        }

        if (other.gameObject == currentSoil)
        {
            currentSoil = null;
            Debug.Log("EXIT SOIL");
        }
    }
}
