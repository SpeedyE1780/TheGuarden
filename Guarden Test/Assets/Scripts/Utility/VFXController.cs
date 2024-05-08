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
        private VFXPlayState playState;

        private void OnEnable()
        {
            playState.onValueChange += OnStateChanged;

            if (playState.PlayOnEnable)
            {
                effect.Play();
            }
        }

        private void OnDisable()
        {
            playState.onValueChange -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            if (playState.PlayOnEnable)
            {
                effect.Play();
            }
            else
            {
                effect.Stop();
            }
        }
    }
}
