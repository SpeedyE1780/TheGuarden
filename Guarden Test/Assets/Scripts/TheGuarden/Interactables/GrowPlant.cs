using UnityEngine;
using UnityEngine.Events;
using TheGuarden.Utility;

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
        [Range(0, 1)]
        public float minimumDryWetRatio;
    }

    [SerializeField]
    private Vector3 startSize;
    [SerializeField]
    private Vector3 maxSize;
    [SerializeField]
    private GrowingInfo growingInfo;
    [SerializeField]
    private GameTime gameTime;
    [SerializeField]
    private ParticleSystem growingParticles;

#if UNITY_EDITOR
    [SerializeField]
    private Transform behaviorParent;
#endif

    public UnityEvent OnFullyGrown;

    private float growthRate = 1.1f;
    private Vector3 targetGrowth = Vector3.zero;
    private bool isGrowing = false;
    private PlantSoil soil;

    public bool IsFullyGrown => transform.localScale == maxSize;
    public float GrowthPercentage => InverseLerp(startSize, maxSize, transform.localScale);
    private bool IsGrowing => isGrowing && soil.DryWetRatio >= growingInfo.minimumDryWetRatio;

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    void Update()
    {
        if (IsGrowing)
        {
            targetGrowth.x = Mathf.Clamp(transform.localScale.x * growthRate, 0, maxSize.x);
            targetGrowth.y = Mathf.Clamp(transform.localScale.y * growthRate, 0, maxSize.y);
            targetGrowth.z = Mathf.Clamp(transform.localScale.z * growthRate, 0, maxSize.z);

            transform.localScale = Vector3.MoveTowards(transform.localScale, targetGrowth, Time.deltaTime * growthRate);
        }
    }

    private void LateUpdate()
    {

        growthRate = gameTime.Hour >= growingInfo.startHour && gameTime.Hour <= growingInfo.endHour ?
            growingInfo.peakGrowingRate :
            growingInfo.offPeakGrowingRate;

        if (IsGrowing && growingParticles.isStopped)
        {
            growingParticles.Play();
        }
        else if (!IsGrowing && growingParticles.isPlaying)
        {
            growingParticles.Stop();
        }

        if (isGrowing && IsFullyGrown)
        {
            isGrowing = false;
            growingParticles.Stop();
            OnFullyGrown?.Invoke();
        }
    }

    public void PickUp()
    {
        if (isGrowing)
        {
            transform.localScale = startSize;
            isGrowing = false;
        }
    }

    public void PlantInSoil(PlantSoil plantSoil)
    {
        isGrowing = true;
        soil = plantSoil;
    }

    private void OnValidate()
    {
        transform.localScale = startSize;

        if (growingInfo.endHour < growingInfo.startHour)
        {
            growingInfo.endHour = growingInfo.startHour;
        }

        gameTime = FindObjectOfType<GameTime>();

        if (gameTime == null)
        {
            GameLogger.LogWarning("GameTime not available in scene", gameObject, GameLogger.LogCategory.Plant);
        }

        growingParticles = GetComponentInChildren<ParticleSystem>();

#if UNITY_EDITOR
        if (behaviorParent != null)
        {
            behaviorParent.localScale = new Vector3(1 / maxSize.x, 1 / maxSize.y, 1 / maxSize.z);
        }
#endif
    }
}
