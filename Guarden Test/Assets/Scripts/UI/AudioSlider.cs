using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    /// <summary>
    /// AudioSlider updates the selected audio mixer parameter
    /// </summary>
    [RequireComponent(typeof(Slider))]
    internal class AudioSlider : MonoBehaviour
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

        /// <summary>
        /// Called from UI Slider and updates the value of the audio mixer parameter
        /// </summary>
        /// <param name="value">New param value</param>
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
