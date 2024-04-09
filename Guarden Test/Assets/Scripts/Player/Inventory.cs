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

    private List<Mushroom> items = new List<Mushroom>();
    private GameObject currentPlant;
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
        Mushroom mushroom = items[SelectedItem];
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
            Mushroom mushroom = items[SelectedItem];

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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentPlant != null)
        {
            Debug.Log("STARTED INTERACTION PICKUP");

            Mushroom mushroom = currentPlant.GetComponent<Mushroom>();

            if (mushroom.IsFullyGrown || mushroom.GrowthPercentage == 0)
            {
                items.Add(mushroom);
                mushroom.PickUp();
                currentPlant = null;
            }
        }

        if (context.performed && currentPlant != null)
        {
            Debug.Log("PERFORMED INTERACTION");

            Mushroom mushroom = currentPlant.GetComponent<Mushroom>();
            items.Add(mushroom);
            mushroom.PickUp();
            currentPlant = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plant") && currentPlant == null)
        {
            currentPlant = other.gameObject;
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
        if (other.gameObject == currentPlant)
        {
            currentPlant = null;
            Debug.Log("EXIT PLANT");
        }

        if (other.gameObject == currentSoil)
        {
            currentSoil = null;
            Debug.Log("EXIT SOIL");
        }
    }
}
