using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    [SerializeField]
    private List<PlantBehavior> behaviors = new List<PlantBehavior>();

    public bool growing = false, grown = false;
    public Vector3 startSize, maxSize;
    public float growthRate = 1.1f;
    public int minutesAtSpawn = 0, elapsedMinutes = 0;

    private Vector3 targetGrowth = Vector3.zero;

    public float GrowthPercentage => InverseLerp(startSize, maxSize, transform.localScale);

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    // Start is called before the first frame update
    void Start()
    {
        minutesAtSpawn = GameTime.totalPassedMinutes;
    }

    // Update is called once per frame
    void Update()
    {
        if (growing && !grown)
        {
            if (GameTime.totalPassedMinutes - minutesAtSpawn > elapsedMinutes && transform.localScale.x < maxSize.x)
            {
                elapsedMinutes = GameTime.totalPassedMinutes - minutesAtSpawn;
                targetGrowth.x = Mathf.Clamp(transform.localScale.x * growthRate, 0, maxSize.x);
                targetGrowth.y = Mathf.Clamp(transform.localScale.y * growthRate, 0, maxSize.y);
                targetGrowth.z = Mathf.Clamp(transform.localScale.z * growthRate, 0, maxSize.z);
            }
            transform.localScale = Vector3.Lerp(transform.localScale, targetGrowth, Time.deltaTime * growthRate);
        }
        if (Vector3.Distance(transform.localScale, maxSize) < 0.001f) { transform.localScale = maxSize; growing = false; grown = true; }
    }

    public void PickUp()
    {
        gameObject.SetActive(false);

        if (!grown)
        {
            transform.localScale = startSize;
        }
    }

    public void Plant(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        setGrowing(true);
        gameObject.SetActive(true);

        foreach(PlantBehavior behavior in behaviors)
        {
            behavior.gameObject.SetActive(true);
        }
    }

    public void setGrowing(bool grow)
    {
        growing = grow;
    }
    public bool getGrowing()
    {
        return growing;
    }
    public bool getGrown() { return grown; }
    public void setGrowthRate(float newGrowthRate)
    {
        growthRate = newGrowthRate;
    }
}
