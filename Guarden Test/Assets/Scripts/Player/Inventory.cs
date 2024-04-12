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

    public int SelectedItem { get; set; }

    private void Start()
    {
        inventoryUI.PlayerInventory = this;
        SelectedItem = -1;
        plantingIndicator.Mask = plantBedMask;
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

    private void OnPlantStarted()
    {
        Mushroom mushroom = items[SelectedItem] as Mushroom;
        plantingIndicator.gameObject.SetActive(true);
        plantingIndicator.UpdateMesh(mushroom.Mesh, mushroom.Materials);

        if (!mushroom.IsFullyGrown && currentSoil != null)
        {
            plantingIndicator.transform.position = currentSoil.transform.position;
            plantingIndicator.PlantingInSoil = true;
        }
    }

    private void OnPlantPerformed()
    {
        plantingIndicator.gameObject.SetActive(false);

        if (items.Count > 0 && SelectedItem != -1)
        {
            bool planted = false;
            Mushroom mushroom = items[SelectedItem] as Mushroom;

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
                SelectedItem = -1;
                inventoryUI.HideSelected();
            }
        }
    }

    public void OnPlant(InputAction.CallbackContext context)
    {
        if (context.started && items.Count > 0 && SelectedItem != -1)
        {
            OnPlantStarted();
        }

        if (context.performed)
        {
            OnPlantPerformed();
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
