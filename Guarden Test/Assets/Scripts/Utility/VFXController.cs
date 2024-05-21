using UnityEngine;
using UnityEngine.VFX;

namespace TheGuarden.Utility
{
    /// <summary>
    /// VFXController uses StateToggle to know if it should be played or stopped
    /// </summary>
    [RequireComponent(typeof(VisualEffect))]
    public class VFXController : MonoBehaviour
    {
        [SerializeField, Tooltip("Attached VisualEffect component")]
        private VisualEffect effect;
        [SerializeField, Tooltip("Toggle used to determine if vfx should be played")]
        private StateToggle playState;
        [SerializeField, Tooltip("Enable/Disable VisualEffect component")]
        private bool disableComponent = false;

        private void OnEnable()
        {
            playState.OnValueChange += OnStateChanged;

            if (playState.Value)
            {
                effect.Play();
            }
        }

        private void OnDisable()
        {
            playState.OnValueChange -= OnStateChanged;
        }

        private void OnStateChanged(bool value)
        {
            if (value)
            {
                GameLogger.LogInfo($"{name} played", this, GameLogger.LogCategory.Scene);
                effect.enabled = true;
                effect.Play();
            }
            else
            {
                GameLogger.LogInfo($"{name} stopped", this, GameLogger.LogCategory.Scene);
                effect.enabled = !disableComponent;
                effect.Stop();
            }
        }
    }
}
