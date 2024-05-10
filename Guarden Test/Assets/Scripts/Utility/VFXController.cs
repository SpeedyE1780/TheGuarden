using UnityEngine;
using UnityEngine.VFX;

namespace TheGuarden.Utility
{
    [RequireComponent(typeof(VisualEffect))]
    public class VFXController : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect effect;
        [SerializeField]
        private StateToggle playState;

        private void OnEnable()
        {
            playState.OnValueChange += OnStateChanged;

            if (playState.Toggled)
            {
                effect.Play();
            }
        }

        private void OnDisable()
        {
            playState.OnValueChange -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            if (playState.Toggled)
            {
                GameLogger.LogInfo($"{name} played", this, GameLogger.LogCategory.Scene);
                effect.Play();
            }
            else
            {
                GameLogger.LogInfo($"{name} stopped", this, GameLogger.LogCategory.Scene);
                effect.Stop();
            }
        }
    }
}
