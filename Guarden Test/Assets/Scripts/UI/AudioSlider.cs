using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField, Tooltip("Targeted Audio Mixer")]
        private AudioMixer audioMixer;
        [SerializeField, Tooltip("Parameter that is linked to slider")]
        private string paramName;

        private void Start()
        {
            if (!audioMixer.GetFloat(paramName, out float value))
            {
                GameLogger.LogError($"{audioMixer.name}: {paramName} not found", this, GameLogger.LogCategory.Audio);
                return;
            }

            GetComponent<Slider>().value = value;
        }

        public void UpdateParameter(float value)
        {
            bool modified = audioMixer.SetFloat(paramName, value);

            if (modified)
            {
                GameLogger.LogInfo($"{audioMixer.name}: {paramName} new value {value}", this, GameLogger.LogCategory.Audio);
            }
            else
            {
                GameLogger.LogError($"{audioMixer.name}: {paramName} wasn't modified", this, GameLogger.LogCategory.Audio);
            }
        }
    }
}
