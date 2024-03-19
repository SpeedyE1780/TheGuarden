using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    [System.Serializable]
    private struct GrowingInfo
    {
        [Range(0, 23)]
        public int startHour;
        [Range(0, 23)]
        public int endHour;
        public float peakGrowingRate;
        public float offPeakGrowingRate;

    }

    [SerializeField]
    private Vector3 startSize;
    [SerializeField]
    private Vector3 maxSize;
    [SerializeField]
    GrowingInfo growingHours;

    private float growthRate = 1.1f;
    private Vector3 targetGrowth = Vector3.zero;

    public bool IsGrowing { get; set; }
    public bool IsFullyGrown => transform.localScale == maxSize;

    public float GrowthPercentage => InverseLerp(startSize, maxSize, transform.localScale);

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    void Start()
    {
        IsGrowing = false;
    }

    void Update()
    {
        if (IsGrowing)
        {
            targetGrowth.x = Mathf.Clamp(transform.localScale.x * growthRate, 0, maxSize.x);
            targetGrowth.y = Mathf.Clamp(transform.localScale.y * growthRate, 0, maxSize.y);
            targetGrowth.z = Mathf.Clamp(transform.localScale.z * growthRate, 0, maxSize.z);

            transform.localScale = Vector3.MoveTowards(transform.localScale, targetGrowth, Time.deltaTime * growthRate);
            IsGrowing = !IsFullyGrown;
        }
    }

    private void LateUpdate()
    {
        if (GameTime.hour >= growingHours.startHour && GameTime.hour <= growingHours.endHour)
        {
            growthRate = growingHours.peakGrowingRate;
        }
        else
        {
            growthRate = growingHours.offPeakGrowingRate;
        }
    }

    public void PickUp()
    {
        if (IsGrowing)
        {
            transform.localScale = startSize;
            IsGrowing = false;
        }
    }

    private void OnValidate()
    {
        transform.localScale = startSize;

        if (growingHours.endHour < growingHours.startHour)
        {
            growingHours.endHour = growingHours.startHour;
        }
    }
}
