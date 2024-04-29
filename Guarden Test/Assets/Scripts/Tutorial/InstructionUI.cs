using UnityEngine;
using TMPro;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// UI spawned when showing instructions on screen
    /// </summary>
    internal class InstructionUI : MonoBehaviour
    {
        [SerializeField, Tooltip("Instruction text")]
        private TextMeshProUGUI instructionText;

        /// <summary>
        /// Set instructions text
        /// </summary>
        /// <param name="instruction">New Instructions</param>
        internal void SetText(string instruction)
        {
            instructionText.text = instruction;
        }
    }
}
