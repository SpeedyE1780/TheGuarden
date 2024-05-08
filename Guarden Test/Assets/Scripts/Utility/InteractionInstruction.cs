using UnityEngine;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Interaction Instruction")]
    public class InteractionInstruction : ScriptableObject
    {
        private const string KeyboardMouseScheme = "Keyboard/Mouse";

        [SerializeField, Multiline]
        private string keyboardInstruction;
        [SerializeField, Multiline]
        private string controllerInstruction;

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
