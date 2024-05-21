using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// InstancedMaterialController will find the instanced material and change it's color
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class InstancedMaterialController : MonoBehaviour
    {
        [SerializeField, Tooltip("Mesh Renderer who's material will change")]
        private MeshRenderer meshRenderer;
        [SerializeField, Tooltip("Material To Change")]
        private Material material;
        [SerializeField, Tooltip("Shader color property")]
        private ExposedProperty colorProperty;

        private void Start()
        {
            foreach (Material mat in meshRenderer.materials)
            {
                if (mat.name.Contains(material.name))
                {
                    GameLogger.LogInfo($"Instanced version {mat.name} of {material.name} found", this, GameLogger.LogCategory.Scene);
                    material = mat;
                    return;
                }
            }

            GameLogger.LogError($"Instanced version of {material.name} not found", this, GameLogger.LogCategory.Scene);
        }

        /// <summary>
        /// Update color of instanced material
        /// </summary>
        /// <param name="color">New material color</param>
        public void UpdateColor(Color color)
        {
            material.SetVector(colorProperty.PropertyID, color);
        }
    }
}
