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
        [SerializeField, Tooltip("Reference to player control scheme")]
        private StringReference playerControlScheme;

        private bool dayCycle = false;

        private void OnEnable()
        {
            playerControlScheme.OnValueChange += UpdateInstructionText;
        }

        private void OnDisable()
        {
            playerControlScheme.OnValueChange -= UpdateInstructionText;
        }

        /// <summary>
        /// Update the message based on the player control scheme
        /// </summary>
        /// <param name="controlScheme">Current control scheme</param>
        private void UpdateInstructionText(string controlScheme)
        {
            instructionText.text = instruction.GetInstructionMessage(controlScheme);
        }

        /// <summary>
        /// Called from game event
        /// </summary>
        public void OnGameStarted()
        {
            UpdateInstructionText(playerControlScheme.Value);
            OnDayStarted();
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
