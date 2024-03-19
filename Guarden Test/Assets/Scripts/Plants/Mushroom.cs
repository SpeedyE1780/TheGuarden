using UnityEngine;

public class Mushroom : MonoBehaviour
{
    GrowPlant plantScript;
    public int growHoursMin,growHoursMax;
    // Start is called before the first frame update
    void Start()
    {
        plantScript = GetComponent<GrowPlant>();
        plantScript.setGrowthRate(1.001f);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameTime.hour >= growHoursMin && GameTime.hour <= growHoursMax) 
        {
            plantScript.setGrowthRate(1.1f);
        }
        else 
        {
            plantScript.setGrowthRate(1.001f);
        }
    }
}
