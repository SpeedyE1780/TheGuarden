using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject plantLocation;
    [SerializeField]
    private InventoryUI inventoryUI;

    private List<GrowPlant> items = new List<GrowPlant>();
    private GameObject currentPlant;

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

            if (items.Count > 0 && SelectedItem != -1 && !items[SelectedItem].getGrown())
            {
                items[SelectedItem].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                items[SelectedItem].transform.position = plantLocation.transform.position;
                items[SelectedItem].transform.rotation = plantLocation.transform.rotation;
                items[SelectedItem].setGrowing(true);
                items[SelectedItem].gameObject.SetActive(true);
                items.Remove(items[SelectedItem]);
                SelectedItem = -1;
                inventoryUI.HideSelected();
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

            GrowPlant plant = currentPlant.GetComponent<GrowPlant>();
            items.Add(plant);
            plant.PickUp();
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentPlant)
        {
            currentPlant = null;
            Debug.Log("EXIT PLANT");
        }
    }
}
