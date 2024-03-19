using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    [SerializeField]
    private List<PlantBehavior> behaviors = new List<PlantBehavior>();
    [SerializeField]
    private Vector3 startSize;
    [SerializeField]
    private Vector3 maxSize;

    public float growthRate = 1.1f;
    public int minutesAtSpawn = 0, elapsedMinutes = 0;

    private Vector3 targetGrowth = Vector3.zero;

    public bool IsGrowing { get; private set; }

    public bool IsFullyGrown => transform.localScale == maxSize;

#if UNITY_EDITOR
    [SerializeField] private Transform behaviorsParent;
#endif

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
        IsGrowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrowing)
        {
            if (GameTime.totalPassedMinutes - minutesAtSpawn > elapsedMinutes && transform.localScale.x < maxSize.x)
            {
                elapsedMinutes = GameTime.totalPassedMinutes - minutesAtSpawn;
                targetGrowth.x = Mathf.Clamp(transform.localScale.x * growthRate, 0, maxSize.x);
                targetGrowth.y = Mathf.Clamp(transform.localScale.y * growthRate, 0, maxSize.y);
                targetGrowth.z = Mathf.Clamp(transform.localScale.z * growthRate, 0, maxSize.z);
            }

            transform.localScale = Vector3.MoveTowards(transform.localScale, targetGrowth, Time.deltaTime * growthRate);
            IsGrowing = !IsFullyGrown;
        }
    }

    public void PickUp()
    {
        gameObject.SetActive(false);

        if (IsGrowing)
        {
            transform.localScale = startSize;
            IsGrowing = false;
        }
    }

    public void Plant(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        IsGrowing = true;
        gameObject.SetActive(true);

        foreach (PlantBehavior behavior in behaviors)
        {
            behavior.gameObject.SetActive(true);
        }
    }

    public void setGrowthRate(float newGrowthRate)
    {
        growthRate = newGrowthRate;
    }

    private void OnValidate()
    {
        transform.localScale = startSize;

        if (behaviorsParent != null)
        {
            behaviors.Clear();

            foreach (Transform behavior in behaviorsParent)
            {
                behaviors.Add(behavior.GetComponent<PlantBehavior>());
            }
        }
    }
}
