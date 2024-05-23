using UnityEngine;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Instructions/Instruction")]
    public class Instruction : ScriptableObject
    {
        [SerializeField, Multiline, Tooltip("Instruction shown on screen")]
        protected string instructions;

        public virtual string GetInstructionMessage()
        {
            return instructions;
        }
    }
}
