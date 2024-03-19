using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject plantLocation;
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

    public void OnPlant(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            plantLocation.SetActive(true);
        }

        if (context.performed)
        {
            Debug.Log("ON PLANT");
            plantLocation.SetActive(false);

            if (items.Count > 0 && SelectedItem != -1)
            {
                bool planted = false;

                if (items[SelectedItem].IsFullyGrown)
                {
                    Debug.Log("Plant anywhere");

                    if(Physics.CheckSphere(plantLocation.transform.position, 2.0f, plantBedMask))
                    {
                        Debug.Log("Can't plant in planting bed");
                        return;
                    }

                    items[SelectedItem].Plant(plantLocation.transform.position, plantLocation.transform.rotation);
                    planted = true;
                }
                else if(currentSoil != null)
                {
                    Debug.Log("Plant in soil");
                    items[SelectedItem].PlantInSoil(currentSoil.transform.position, currentSoil.transform.rotation);
                    planted = true;
                }

                if (planted)
                {
                    items.Remove(items[SelectedItem]);
                    SelectedItem = -1;
                    inventoryUI.HideSelected(); 
                }
            }
        }

        if (context.canceled)
        {
            plantLocation.SetActive(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
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
