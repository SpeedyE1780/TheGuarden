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
    [SerializeField]
    private LayerMask lakeLayer;

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

    public void ShowPlantingIndicator(Mushroom mushroom)
    {
        plantingIndicator.gameObject.SetActive(true);
        plantingIndicator.UpdateMesh(mushroom.Mesh, mushroom.Materials);

        if (!mushroom.IsFullyGrown && currentSoil != null)
        {
            plantingIndicator.transform.position = currentSoil.transform.position;
            plantingIndicator.PlantingInSoil = true;
        }
    }

    public void PlantMushroom(Mushroom mushroom)
    {
        plantingIndicator.gameObject.SetActive(false);
        bool planted = false;

        if (mushroom.IsFullyGrown)
        {
            GameLogger.LogInfo("Plant anywhere", gameObject);

            if (Physics.CheckSphere(plantingIndicator.transform.position, 2.0f, plantBedMask))
            {
                GameLogger.LogWarning("Can't plant in planting bed", gameObject);
                return;
            }

            mushroom.Plant(plantingIndicator.transform.position, plantingIndicator.transform.rotation);
            planted = true;
        }
        else if (currentSoil != null)
        {
            GameLogger.LogInfo("Plant in soil", gameObject);
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

    public void FillWaterBucket(Bucket bucket)
    {
        if (Physics.CheckSphere(transform.position, 2.0f, lakeLayer))
        {
            bucket.AddWater();
            GameLogger.LogInfo("Adding water to bucket", gameObject);
        }
    }

    public void WaterSoil(Bucket bucket)
    {
        if (currentSoil != null)
        {
            GameLogger.LogInfo("Remove water from bucket", gameObject);
            bucket.RemoveWater();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && selectedItem != null)
        {
            selectedItem.OnInteractionStarted(this);
        }

        if (context.performed)
        {
            selectedItem.OnInteractionPerformed(this);
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
            GameLogger.LogInfo("STARTED INTERACTION PICKUP", gameObject);

            IInteractable interactable = currentInteractable.GetComponent<IInteractable>();

            if (interactable.HasInstantPickUp)
            {
                items.Add(interactable);
                interactable.PickUp();
                currentInteractable = null;
            }
        }

        if (context.performed && currentInteractable != null)
        {
            GameLogger.LogInfo("PERFORMED PICKUP", gameObject);

            IInteractable interactable = currentInteractable.GetComponent<IInteractable>();
            items.Add(interactable);
            interactable.PickUp();
            currentInteractable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Plant") || other.CompareTag("Bucket")) && currentInteractable == null)
        {
            currentInteractable = other.gameObject;
            GameLogger.LogInfo("ENTER PLANT", gameObject);
        }

        if (other.CompareTag("PlantSoil") && currentSoil == null)
        {
            currentSoil = other.gameObject;
            GameLogger.LogInfo("ENTER SOIL", gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentInteractable)
        {
            currentInteractable = null;
            GameLogger.LogInfo("EXIT PLANT", gameObject);
        }

        if (other.gameObject == currentSoil)
        {
            currentSoil = null;
            GameLogger.LogInfo("EXIT SOIL", gameObject);
        }
    }
}
