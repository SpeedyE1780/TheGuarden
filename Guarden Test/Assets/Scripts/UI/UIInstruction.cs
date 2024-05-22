using TheGuarden.Utility;
using TMPro;
using UnityEngine;

namespace TheGuarden.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal class UIInstruction : MonoBehaviour
    {
        [SerializeField, Tooltip("Active control scheme")]
        private StringReference controlScheme;
        [SerializeField, Tooltip("Instruction text")]
        private TextMeshProUGUI instructionText;
        [SerializeField, Tooltip("Instruction SO")]
        private InteractionInstruction instructionInteraction;

        private void OnEnable()
        {
            controlScheme.OnValueChange += UpdateInstruction;
            UpdateInstruction(controlScheme.Value);
        }

        private void OnDisable()
        {
            controlScheme.OnValueChange -= UpdateInstruction;
        }

        private void UpdateInstruction(string controlScheme)
        {
            instructionText.text = instructionInteraction.GetInstructionMessage(controlScheme);
        }
    }
}
