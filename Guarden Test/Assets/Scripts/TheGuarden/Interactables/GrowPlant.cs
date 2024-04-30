using UnityEngine;
using UnityEngine.Events;
using TheGuarden.Utility;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// GrowPlant controls the mushroom growing speed
    /// </summary>
    internal class GrowPlant : MonoBehaviour
    {
        /// <summary>
        /// GrowingInfo has all info related to growing
        /// </summary>
        [System.Serializable]
        private struct GrowingInfo
        {
            [Tooltip("Peak rate at which plant grows")]
            public float peakGrowingRate;
            [Tooltip("Off peak rate at which plant grows")]
            public float offPeakGrowingRate;
            [Range(0, 1), Tooltip("Minimum ratio needed to grow")]
            public float minimumDryWetRatio;
            [Tooltip("Size when growing starts")]
            public Vector3 startSize;
            [Tooltip("Size when growing ends")]
            public Vector3 maxSize;
        }

        [SerializeField, Tooltip("All growing related info")]
        private GrowingInfo growingInfo;
        [SerializeField, Tooltip("Autofilled. Particle system played while plant grows")]
        private ParticleSystem growingParticles;

        public UnityEvent OnFullyGrown;

        private float growthRate = 1.1f;
        private Vector3 targetGrowth = Vector3.zero;
        private bool isGrowing = false;
        private PlantSoil soil;

        public bool IsFullyGrown => transform.localScale == growingInfo.maxSize;
        public float GrowthPercentage => MathExtensions.InverseLerp(growingInfo.startSize, growingInfo.maxSize, transform.localScale);
        private bool IsGrowing => isGrowing && soil.DryWetRatio >= growingInfo.minimumDryWetRatio;

#if UNITY_EDITOR
        internal Vector3 MaxSize => growingInfo.maxSize;
#endif

        private void Awake()
        {
            growthRate = growingInfo.peakGrowingRate;
        }

        private void OnEnable()
        {
            DayLightCycle.OnDayStarted += UsePeakRate;
            DayLightCycle.OnNightStarted += UseOffPeakRate;
        }

        private void OnDisable()
        {
            DayLightCycle.OnDayStarted -= UsePeakRate;
            DayLightCycle.OnNightStarted -= UseOffPeakRate;
        }

        void Update()
        {
            if (IsGrowing)
            {
                targetGrowth.x = Mathf.Clamp(transform.localScale.x + (growthRate * growingInfo.startSize.x), 0, growingInfo.maxSize.x);
                targetGrowth.y = Mathf.Clamp(transform.localScale.y + (growthRate * growingInfo.startSize.y), 0, growingInfo.maxSize.y);
                targetGrowth.z = Mathf.Clamp(transform.localScale.z + (growthRate * growingInfo.startSize.z), 0, growingInfo.maxSize.z);

                transform.localScale = Vector3.MoveTowards(transform.localScale, targetGrowth, Time.deltaTime * growthRate);
            }
        }

        private void LateUpdate()
        {
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

        private void UsePeakRate()
        {
            growthRate = growingInfo.peakGrowingRate;
        }

        private void UseOffPeakRate()
        {
            growthRate = growingInfo.offPeakGrowingRate;
        }

        /// <summary>
        /// Stop growing and reset scale
        /// </summary>
        public void ResetGrowing()
        {
            if (isGrowing)
            {
                transform.localScale = growingInfo.startSize;
                isGrowing = false;
            }
        }

        /// <summary>
        /// Start growing
        /// </summary>
        /// <param name="plantSoil">Soil in which plant is planted</param>
        public void PlantInSoil(PlantSoil plantSoil)
        {
            isGrowing = true;
            soil = plantSoil;
        }

#if UNITY_EDITOR
        internal Transform GetBehaviorParent()
        {
            Transform behaviorParent = transform.Find("Behaviors");

            if (behaviorParent == null)
            {
                GameLogger.LogError("GrowPlant doesn't have Behaviors child", this, GameLogger.LogCategory.Scene);
            }

            return behaviorParent;
        }

        internal void AutofillVariables()
        {
            growingParticles = GetComponentInChildren<ParticleSystem>();
        }
#endif
    }
}
