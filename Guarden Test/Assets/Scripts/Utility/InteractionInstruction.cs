using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Contains keyboard and controller instructions
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Interaction Instruction")]
    public class InteractionInstruction : ScriptableObject
    {
        private const string KeyboardMouseScheme = "Keyboard/Mouse";

        [SerializeField, Multiline, Tooltip("Keyboard instruction")]
        private string keyboardInstruction;
        [SerializeField, Multiline, Tooltip("Controller instruction")]
        private string controllerInstruction;

        /// <summary>
        /// Return instruction based on control scheme
        /// </summary>
        /// <param name="controlScheme">Current player control scheme</param>
        /// <returns>Instructions based on control scheme</returns>
        public string GetInstructionMessage(string controlScheme)
        {
            if (controlScheme == KeyboardMouseScheme)
            {
                return keyboardInstruction;
            }

            return controllerInstruction;
        }
    }
}
