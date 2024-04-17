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
    [SerializeField]
    private float overlapRadius = 2.0f;

    private List<IInteractable> items = new List<IInteractable>();
    private GameObject currentInteractable;
    private GameObject currentSoil;
    private int selectedItemIndex;

    private void Start()
    {
        selectedItemIndex = -1;
        plantingIndicator.Mask = plantBedMask;
    }

    public void SetInventoryUI(InventoryUI UI)
    {
        inventoryUI = UI;
        inventoryUI.gameObject.SetActive(true);
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
            GameLogger.LogInfo("Plant anywhere", gameObject, GameLogger.LogCategory.Player);

            if (Physics.CheckSphere(plantingIndicator.transform.position, overlapRadius, plantBedMask))
            {
                GameLogger.LogWarning("Can't plant in planting bed", gameObject, GameLogger.LogCategory.Player);
                return;
            }

            mushroom.Plant(plantingIndicator.transform.position, plantingIndicator.transform.rotation);
            planted = true;
        }
        else if (currentSoil != null)
        {
            GameLogger.LogInfo("Plant in soil", gameObject, GameLogger.LogCategory.Player);
            PlantSoil soil = currentSoil.GetComponent<PlantSoil>();
            mushroom.PlantInSoil(soil, currentSoil.transform.position, currentSoil.transform.rotation);
            planted = true;
        }

        if (planted)
        {
            items.Remove(mushroom);
            inventoryUI.RemoveItem(selectedItemIndex);
            selectedItemIndex = -1;
        }
    }

    public void FillWaterBucket(Bucket bucket)
    {
        if (Physics.CheckSphere(transform.position, overlapRadius, lakeLayer))
        {
            bucket.AddWater();
            GameLogger.LogInfo("Adding water to bucket", gameObject, GameLogger.LogCategory.Player);
        }
    }

    public void WaterPlantBed(Bucket bucket)
    {
        Collider[] plantBedsCollider = new Collider[1];
        if (Physics.OverlapSphereNonAlloc(transform.position, overlapRadius, plantBedsCollider, plantBedMask) > 0)
        {
            PlantBed plantBed = plantBedsCollider[0].GetComponent<PlantBed>();
            bucket.WaterPlantBed(plantBed);
            GameLogger.LogInfo("Watering plant bed", gameObject, GameLogger.LogCategory.Player);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        IInteractable selectedItem = selectedItemIndex >= 0 && selectedItemIndex < items.Count ? items[selectedItemIndex] : null;

        if (context.started && selectedItem != null)
        {
            selectedItem.OnInteractionStarted(this);
        }

        if (context.performed && selectedItem != null)
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
            GameLogger.LogInfo("STARTED INTERACTION PICKUP", gameObject, GameLogger.LogCategory.Player);

            IInteractable interactable = currentInteractable.GetComponent<IInteractable>();

            if (interactable.HasInstantPickUp)
            {
                items.Add(interactable);
                inventoryUI.AddItem(interactable);
                interactable.PickUp();
                currentInteractable = null;
            }
        }

        if (context.performed && currentInteractable != null)
        {
            GameLogger.LogInfo("PERFORMED PICKUP", gameObject, GameLogger.LogCategory.Player);

            IInteractable interactable = currentInteractable.GetComponent<IInteractable>();
            items.Add(interactable);
            interactable.PickUp();
            inventoryUI.AddItem(interactable);
            currentInteractable = null;
        }

        if (selectedItemIndex == -1 && items.Count > 0)
        {
            selectedItemIndex = 0;
            inventoryUI.SelectItem(0);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (Tags.HasTag(other.gameObject, Tags.Plant, Tags.Bucket) && currentInteractable == null)
        {
            currentInteractable = other.gameObject;
            GameLogger.LogInfo("ENTER PLANT", gameObject, GameLogger.LogCategory.Player);
        }

        if (other.CompareTag(Tags.PlantSoil) && currentSoil == null)
        {
            currentSoil = other.gameObject;
            GameLogger.LogInfo("ENTER SOIL", gameObject, GameLogger.LogCategory.Player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentInteractable)
        {
            currentInteractable = null;
            GameLogger.LogInfo("EXIT PLANT", gameObject, GameLogger.LogCategory.Player);
        }

        if (other.gameObject == currentSoil)
        {
            currentSoil = null;
            GameLogger.LogInfo("EXIT SOIL", gameObject, GameLogger.LogCategory.Player);
        }
    }
}
