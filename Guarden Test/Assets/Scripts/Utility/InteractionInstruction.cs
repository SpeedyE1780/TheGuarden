using UnityEngine;
using UnityEngine.InputSystem;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Contains keyboard and controller instructions
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Interaction Instruction")]
    public class InteractionInstruction : ScriptableObject
    {
        [SerializeField, Multiline, Tooltip("Instruction shown on screen")]
        private string instructions;
        [SerializeField, Tooltip("Instruction's input action")]
        private InputActionReference actionReference;

        /// <summary>
        /// Return instruction based on control scheme
        /// </summary>
        /// <param name="controlScheme">Current player control scheme</param>
        /// <returns>Instructions based on control scheme</returns>
        public string GetInstructionMessage(string controlScheme)
        {
            string bindingDisplay = actionReference != null ? actionReference.action.GetBindingDisplayString(InputBinding.MaskByGroup(controlScheme), InputBinding.DisplayStringOptions.DontIncludeInteractions) : string.Empty;
            return string.Format(instructions, bindingDisplay);
        }

        public string GetAllBindingsInstructionMessage()
        {
            string bindingDisplay = actionReference != null ? actionReference.action.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions) : string.Empty;
            return string.Format(instructions, bindingDisplay);
        }
    }
}
