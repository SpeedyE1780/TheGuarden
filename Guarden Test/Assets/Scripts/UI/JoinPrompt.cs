using TheGuarden.Utility;
using TMPro;
using UnityEngine;

namespace TheGuarden.UI
{
    /// <summary>
    /// JoinPrompt will show a prompt telling player to join the game
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal class JoinPrompt : MonoBehaviour
    {
        [SerializeField, Tooltip("Joining instructions")]
        private InteractionInstruction joinInstruction;
        [SerializeField, Tooltip("Player in scene state")]
        private StateToggle playerInScene;
        [SerializeField, Tooltip("Prompt text")]
        private TextMeshProUGUI instructionText;

        private void OnEnable()
        {
            playerInScene.OnValueChange += TogglePrompt;
            TogglePrompt();
        }

        private void OnDisable()
        {
            playerInScene.OnValueChange -= TogglePrompt;
        }

        /// <summary>
        /// Toggle text state based on if players are in scene
        /// </summary>
        private void TogglePrompt()
        {
            instructionText.enabled = !playerInScene.Toggled;
        }

#if UNITY_EDITOR
        internal void SetText()
        {
            if (joinInstruction != null)
            {
                instructionText.text = joinInstruction.GetInstructionMessage(string.Empty);
            }
        }
#endif
    }
}
