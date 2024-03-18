using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items;
    int selectedItem = 0;
    GameObject currentPlant;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (items.Count > 0 && !items[selectedItem].GetComponent<GrowPlant>().getGrown())
            {
                items[selectedItem].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                items[selectedItem].transform.position = transform.position;
                items[selectedItem].transform.rotation = transform.rotation;
                items[selectedItem].GetComponent<GrowPlant>().setGrowing(true);
                items[selectedItem].gameObject.SetActive(true);
                items.Remove(items[selectedItem]);
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && currentPlant != null)
        {
            Debug.Log("PERFORMED INTERACTION");

            currentPlant.GetComponent<GrowPlant>().PickUp();
            AddItemToInventory(currentPlant);
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

    private void AddItemToInventory(GameObject item)
    {
        items.Add(item);
    }
}
