using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject plantLocation;

    private List<GrowPlant> items = new List<GrowPlant>();
    private int selectedItem = 0;
    private GameObject currentPlant;

    public void OnPlant(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            plantLocation.SetActive(true);
        }

        if (context.performed)
        {
            Debug.Log("ON PLANT");
            plantLocation.SetActive(false);

            if (items.Count > 0 && !items[selectedItem].getGrown())
            {
                items[selectedItem].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                items[selectedItem].transform.position = plantLocation.transform.position;
                items[selectedItem].transform.rotation = plantLocation.transform.rotation;
                items[selectedItem].setGrowing(true);
                items[selectedItem].gameObject.SetActive(true);
                items.Remove(items[selectedItem]);
            }
        }

        if(context.canceled)
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
