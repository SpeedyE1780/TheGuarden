using System.Collections;
using TheGuarden.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    /// <summary>
    /// DayLightProgress updates a slider to show when night will start
    /// </summary>
    public class DayLightProgress : MonoBehaviour
    {
        [SerializeField, Tooltip("Slider showing day progress")]
        private Slider dayProgress;
        [SerializeField, Tooltip("DayLight cycle in scene")]
        private DayLightCycle cycle;
        [SerializeField, Tooltip("Text showing start wave instructions")]
        private TextMeshProUGUI instructionText;
        [SerializeField, Tooltip("StartWave early instruction text")]
        private InteractionInstruction instruction;

        private bool dayCycle = false;

        private void Start()
        {
            instructionText.text = instruction.GetAllBindingsInstructionMessage();
        }

        /// <summary>
        /// Called from game event
        /// </summary>
        public void OnDayStarted()
        {
            dayCycle = true;
            instructionText.gameObject.SetActive(true);
            StartCoroutine(ShowDayInstructions());
        }

        /// <summary>
        /// Called from game event
        /// </summary>
        public void OnNightStarted()
        {
            dayCycle = false;
            instructionText.gameObject.SetActive(false);
        }

        /// <summary>
        /// Update slider during day cycle
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowDayInstructions()
        {
            while (dayCycle)
            {
                dayProgress.value = cycle.DayProgess;
                yield return null;
            }

            dayProgress.value = cycle.DayProgess;
        }
    }
}
