using UnityEngine;
using UnityEngine.Events;
using TheGuarden.Utility;

namespace TheGuarden.Interactable
{
    internal class GrowPlant : MonoBehaviour
    {
        /// <summary>
        /// GrowingInfo has all info related to growing
        /// </summary>
        [System.Serializable]
        private struct GrowingInfo
        {
            [Range(0, 23), Tooltip("Hour where peak growing starts")]
            public int startHour;
            [Range(0, 23), Tooltip("Hour where peak growing ends")]
            public int endHour;
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
        [SerializeField, Tooltip("Autofilled. GameTime in scene")]
        private GameTime gameTime;
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

        void Update()
        {
            if (IsGrowing)
            {
                targetGrowth.x = Mathf.Clamp(transform.localScale.x * growthRate, 0, growingInfo.maxSize.x);
                targetGrowth.y = Mathf.Clamp(transform.localScale.y * growthRate, 0, growingInfo.maxSize.y);
                targetGrowth.z = Mathf.Clamp(transform.localScale.z * growthRate, 0, growingInfo.maxSize.z);

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

        /// <summary>
        /// Set game time used to grow
        /// </summary>
        /// <param name="time">Game time used to grow</param>
        internal void SetGameTime(GameTime time)
        {
            gameTime = time;
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
        internal void ValidateVariables()
        {
            transform.localScale = growingInfo.startSize;

            if (growingInfo.endHour < growingInfo.startHour)
            {
                growingInfo.endHour = growingInfo.startHour;
            }
        }

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
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("GameTime not available in scene", gameObject, GameLogger.LogCategory.Plant);
            }

            growingParticles = GetComponentInChildren<ParticleSystem>();
        }
#endif
    }
}
