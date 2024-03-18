using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items;
    int selectedItem = 0;
    Transform touchingPlantPoint = null;
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
            if (items.Count > 0 && touchingPlantPoint != null && !items[selectedItem].GetComponent<GrowPlant>().getGrown())
            {
                items[selectedItem].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                items[selectedItem].transform.SetParent(touchingPlantPoint, true);
                items[selectedItem].transform.position = touchingPlantPoint.position;
                items[selectedItem].transform.rotation = touchingPlantPoint.rotation;
                items[selectedItem].GetComponent<GrowPlant>().setGrowing(true);
                items[selectedItem].gameObject.SetActive(true);
                items.Remove(items[selectedItem]);
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("PERFORMED INTERACTION");
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

    public void setTouchingPlantPoint(Transform plantPoint)
    {
        touchingPlantPoint = plantPoint;
    }
    public void addItemToInventory(GameObject item)
    {
        items.Add(item);
    }
}
