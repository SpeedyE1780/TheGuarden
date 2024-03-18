using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items;
    [SerializeField] GameObject testObject;
    int selectedItem = 0;
    Transform touchingPlantPoint = null;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<GameObject>();
        //items.Add(testObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if(items.Count > 0 && touchingPlantPoint != null)
            {
                items[selectedItem].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                items[selectedItem].transform.SetParent(touchingPlantPoint,true);
                items[selectedItem].transform.position = touchingPlantPoint.position;
                items[selectedItem].transform.rotation = touchingPlantPoint.rotation;
                items[selectedItem].GetComponent<GrowPlant>().setGrowing(true);
                items[selectedItem].gameObject.SetActive(true);
                items.Remove(items[selectedItem]);
            }
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
