using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.VFX;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// GrowPlant controls the mushroom growing speed
    /// </summary>
    internal class GrowPlant : MonoBehaviour
    {
        [SerializeField, Tooltip("All growing related info")]
        private GrowingInfo growingInfo;
        [SerializeField, Tooltip("Autofilled. Particle system played while plant grows")]
        private VisualEffect growingParticles;
        [SerializeField, Tooltip("Parent of all gameobject that will grow")]
        private Transform growingTransform;
        [SerializeField, Tooltip("Game event called when mushroom is fully grown")]
        private GameEvent onFullyGrown;
        [SerializeField, Tooltip("Growing event VFX Property")]
        private ShaderProperty onGrowingProperty;
        [SerializeField, Tooltip("StopGrowing event VFX Property")]
        private ShaderProperty onStopGrowingProperty;
        [SerializeField, Tooltip("FullyGrown event VFX Property")]
        private ShaderProperty onFullyGrownProperty;

        private Vector3 targetGrowth = Vector3.zero;
        private bool isGrowing = false;
        private PlantSoil soil;

        public bool IsFullyGrown => GrowthPercentage >= 0.99f;
        public float GrowthPercentage => MathExtensions.InverseLerp(growingInfo.startSize, growingInfo.maxSize, growingTransform.localScale);
        private bool IsGrowing => !IsFullyGrown && soil != null && soil.DryWetRatio >= growingInfo.minimumDryWetRatio;

#if UNITY_EDITOR
        internal Vector3 MaxSize => growingInfo.maxSize;
#endif

        void Update()
        {
            if (IsGrowing)
            {
                targetGrowth.x = Mathf.Clamp(growingTransform.localScale.x + (growingInfo.growthRate * growingInfo.startSize.x), 0, growingInfo.maxSize.x);
                targetGrowth.y = Mathf.Clamp(growingTransform.localScale.y + (growingInfo.growthRate * growingInfo.startSize.y), 0, growingInfo.maxSize.y);
                targetGrowth.z = Mathf.Clamp(growingTransform.localScale.z + (growingInfo.growthRate * growingInfo.startSize.z), 0, growingInfo.maxSize.z);

                growingTransform.localScale = Vector3.MoveTowards(growingTransform.localScale, targetGrowth, Time.deltaTime * growingInfo.growthRate);
            }
        }

        private void LateUpdate()
        {
            if (isGrowing && IsFullyGrown)
            {
                isGrowing = false;
                growingTransform.localScale = growingInfo.maxSize;
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
                growingTransform.localScale = growingInfo.startSize;
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
            soil = plantSoil;
            growingParticles.Play();
        }

        /// <summary>
        /// Reset state before entering pool and reset soil
        /// </summary>
        public void OnEnterPool()
        {
            growingTransform.localScale = growingInfo.startSize;
            isGrowing = false;
            soil = null;
            growingParticles.Stop();
        }


#if UNITY_EDITOR
        internal void AutofillVariables()
        {
            growingParticles = GetComponentInChildren<VisualEffect>();
        }
#endif
    }
}
