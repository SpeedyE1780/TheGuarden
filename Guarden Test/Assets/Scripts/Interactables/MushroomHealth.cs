using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// MushroomHealth will remove one HP from mushroom and change top color to black
    /// </summary>
    [RequireComponent(typeof(Health))]
    internal class MushroomHealth : MonoBehaviour
    {
        [SerializeField, Tooltip("Mushroom health component")]
        private Health health;
        [SerializeField, Tooltip("Mushroom top material")]
        private InstancedMaterialController mushroomTop;
        [SerializeField, Tooltip("Color before mushroom dying")]
        private Color finalColor;

        private Color startColor;

        private void Start()
        {
            startColor = mushroomTop.GetColor();
        }

        private void UpdateTopColor()
        {
            //Subtract one from currentHealth that way mushroom will be fully black on last day
            Color currentColor = Color.Lerp(startColor, finalColor, 1 - (health.CurrentHealth - 1) / health.CurrentMaxHealth);
            mushroomTop.UpdateColor(currentColor);
        }

        public void Decay()
        {
            health.Damage(1);
            UpdateTopColor();
        }
    }
}
