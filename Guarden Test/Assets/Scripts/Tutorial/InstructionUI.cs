using UnityEngine;
using TMPro;

namespace TheGuarden.Tutorial
{
    internal class InstructionUI : MonoBehaviour
    {
        [SerializeField, Tooltip("Instruction text")]
        private TextMeshProUGUI instructionText;

        internal void SetText(string instruction)
        {
            instructionText.text = instruction;
        }
    }
}
