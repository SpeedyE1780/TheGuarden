using UnityEngine;
using TheGuarden.Utility;
using UnityEngine.VFX;
using TheGuarden.Utility.Events;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// GrowPlant controls the mushroom growing speed
    /// </summary>
    internal class GrowPlant : MonoBehaviour, IPoolObject
    {
        [SerializeField, Tooltip("All growing related info")]
        private GrowingInfo growingInfo;
        [SerializeField, Tooltip("Autofilled. Particle system played while plant grows")]
        private VisualEffect growingParticles;
        [SerializeField]
        private GameEvent onFullyGrown;
        [SerializeField]
        private ExposedProperty onGrowingProperty;
        [SerializeField]
        private ExposedProperty onStopGrowingProperty;
        [SerializeField]
        private ExposedProperty onFullyGrownProperty;

        private Vector3 targetGrowth = Vector3.zero;
        private bool isGrowing = false;
        private PlantSoil soil;

        public bool IsFullyGrown => transform.localScale == growingInfo.maxSize;
        public float GrowthPercentage => MathExtensions.InverseLerp(growingInfo.startSize, growingInfo.maxSize, transform.localScale);
        private bool IsGrowing => !IsFullyGrown && soil != null && soil.DryWetRatio >= growingInfo.minimumDryWetRatio;

#if UNITY_EDITOR
        internal Vector3 MaxSize => growingInfo.maxSize;
#endif

        void Update()
        {
            if (IsGrowing)
            {
                targetGrowth.x = Mathf.Clamp(transform.localScale.x + (growingInfo.growthRate * growingInfo.startSize.x), 0, growingInfo.maxSize.x);
                targetGrowth.y = Mathf.Clamp(transform.localScale.y + (growingInfo.growthRate * growingInfo.startSize.y), 0, growingInfo.maxSize.y);
                targetGrowth.z = Mathf.Clamp(transform.localScale.z + (growingInfo.growthRate * growingInfo.startSize.z), 0, growingInfo.maxSize.z);

                transform.localScale = Vector3.MoveTowards(transform.localScale, targetGrowth, Time.deltaTime * growingInfo.growthRate);
            }
        }

        private void LateUpdate()
        {
            if (isGrowing && IsFullyGrown)
            {
                isGrowing = false;
                growingParticles.SendEvent(onStopGrowingProperty.PropertyID);
                growingParticles.SendEvent(onFullyGrownProperty.PropertyID);
                onFullyGrown.Raise();
            }

            if (IsGrowing && !isGrowing)
            {
                isGrowing = true;
                growingParticles.SendEvent(onGrowingProperty.PropertyID);
            }
            else if (!IsGrowing && isGrowing)
            {
                isGrowing = false;
                growingParticles.SendEvent(onStopGrowingProperty.PropertyID);
            }
        }

        /// <summary>
        /// Stop growing and reset scale
        /// </summary>
        public void ResetGrowing()
        {
            if (isGrowing)
            {
                growingParticles.Stop();
                transform.localScale = growingInfo.startSize;
                isGrowing = false;
            }

            soil = null;
        }

        /// <summary>
        /// Start growing
        /// </summary>
        /// <param name="plantSoil">Soil in which plant is planted</param>
        public void PlantInSoil(PlantSoil plantSoil)
        {
            isGrowing = true;
            soil = plantSoil;
            growingParticles.Play();
        }

        public void OnEnterPool()
        {
            transform.localScale = growingInfo.startSize;
            isGrowing = false;
            soil = null;
            growingParticles.Stop();
        }

        public void OnExitPool()
        {
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
            growingParticles = GetComponentInChildren<VisualEffect>();
        }
#endif
    }
}
