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

        //Dry = 0, Wet = 1
        internal float dryWetRatio = 0;
        private Material material;

        private void Start()
        {
            material = GetComponent<Renderer>().material;
            material.color = Color.Lerp(dryColor, wetColor, dryWetRatio);
        }

        private void Update()
        {
            dryWetRatio = Mathf.Clamp01(dryWetRatio - dryingSpeed * Time.deltaTime);
            material.color = Color.Lerp(dryColor, wetColor, dryWetRatio);
        }

        /// <summary>
        /// Restore dryWetRatio
        /// </summary>
        /// <param name="bucketRestoration">Amount added to dryWetRatio</param>
        public void Water(float bucketRestoration)
        {
            dryWetRatio = Mathf.Clamp01(dryWetRatio + bucketRestoration);
        }
    } 
}
