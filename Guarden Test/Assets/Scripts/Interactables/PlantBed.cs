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
        [SerializeField, Tooltip("Mesh Renderer who's material will change")]
        private MeshRenderer renderer;
        [SerializeField, Tooltip("Material To Change")]
        private Material material;
        [SerializeField, Tooltip("Shader color property")]
        private ExposedProperty colorProperty;

        //Dry = 0, Wet = 1
        internal float dryWetRatio = 0;

        private void Start()
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.name.Contains(material.name))
                {
                    material = mat;
                    break;
                }
            }

            UpdateColor();
        }

        private void Update()
        {
            dryWetRatio = Mathf.Clamp01(dryWetRatio - dryingSpeed * Time.deltaTime);
            UpdateColor();
        }

        private void UpdateColor()
        {
            Color color = Color.Lerp(dryColor, wetColor, dryWetRatio);
            material.SetVector(colorProperty.PropertyID, color);
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
