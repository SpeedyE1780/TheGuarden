using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// PlantBed represents the bed where planted are planted
    /// </summary>
    internal class PlantBed : MonoBehaviour
    {
        [SerializeField, Tooltip("Color when bed is dry")]
        private Color dryColor;
        [SerializeField, Tooltip("Color when bed is wet")]
        private Color wetColor;
        [SerializeField, Tooltip("Speed at which bed is drying")]
        private float dryingSpeed = 0.01f;
        [SerializeField, Tooltip("Component updating instanced material of plant bed")]
        private InstancedMaterialController plantBedMaterial;

        //Dry = 0, Wet = 1
        internal float dryWetRatio = 0;

        private void Start()
        {
            UpdateColor();
        }

        private void Update()
        {
            dryWetRatio = Mathf.Clamp01(dryWetRatio - dryingSpeed * Time.deltaTime);
            UpdateColor();
        }

        /// <summary>
        /// Update plant bed color based on dry wet ratio
        /// </summary>
        private void UpdateColor()
        {
            Color color = Color.Lerp(dryColor, wetColor, dryWetRatio);
            plantBedMaterial.UpdateColor(color);
        }

        /// <summary>
        /// Restore dryWetRatio
        /// </summary>
        /// <param name="bucketRestoration">Amount added to dryWetRatio</param>
        internal void Water(float bucketRestoration)
        {
            dryWetRatio = Mathf.Clamp01(dryWetRatio + bucketRestoration);
        }
    }
}
