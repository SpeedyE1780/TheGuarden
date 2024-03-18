using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public bool growing = false;
    public Vector3 startSize,maxSize;
    public float growthRate = 1.1f;
    public int minutesAtSpawn = 0,elapsedMinutes = 0;
    public float keyPressDuration = 0, requiredKeyPressDuration = 1.0f;

    private Vector3 targetGrowth = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        minutesAtSpawn = GameTime.totalPassedMinutes;
    }

    // Update is called once per frame
    void Update()
    {
        if (growing) 
        {
            if(GameTime.totalPassedMinutes - minutesAtSpawn > elapsedMinutes && transform.localScale.x < maxSize.x) 
            {
                elapsedMinutes = GameTime.totalPassedMinutes - minutesAtSpawn;
                targetGrowth.x = Mathf.Clamp(transform.localScale.x * growthRate, 0, maxSize.x);
                targetGrowth.y = Mathf.Clamp(transform.localScale.y * growthRate, 0, maxSize.y);
                targetGrowth.z = Mathf.Clamp(transform.localScale.z * growthRate, 0, maxSize.z);
            }
            transform.localScale = Vector3.Lerp(transform.localScale, targetGrowth, Time.deltaTime * growthRate);
        }
        if(Vector3.Distance(transform.localScale, maxSize) < 0.001f) { transform.localScale = maxSize; growing = false; }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !growing) 
        {
            other.GetComponent<Inventory>().addItemToInventory(gameObject);
            gameObject.SetActive(false);
        }
    }
    */

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.Space)) 
            {
                keyPressDuration += Time.deltaTime;
                if(keyPressDuration >= requiredKeyPressDuration) 
                {
                    keyPressDuration = 0;
                    transform.localScale = startSize;

                    other.GetComponent<Inventory>().addItemToInventory(gameObject);
                    gameObject.SetActive(false);
                }
            }
            else { keyPressDuration = 0; }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { keyPressDuration = 0; }
        
    }

    public void setGrowing(bool grow) 
    {
        growing = grow;
    }
    public bool getGrowing() 
    {
        return growing;
    }
    public void setGrowthRate(float newGrowthRate) 
    {
        growthRate = newGrowthRate;    
    }




}
