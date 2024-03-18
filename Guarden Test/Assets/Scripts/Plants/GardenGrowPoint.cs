using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GardenGrowPoint : MonoBehaviour
{
    //GlobalVariables variables;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            other.GetComponent<Inventory>().setTouchingPlantPoint(this.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Inventory>().setTouchingPlantPoint(null);
        }
    }
}
