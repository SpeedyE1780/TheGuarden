using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Used to keep track of a game state
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Value Reference/State Toggle")]
    public class StateToggle : ValueReference<bool>
    {
        /// <summary>
        /// Activate state
        /// </summary>
        public void TurnOn()
        {
            SetValue(true);
        }

        /// <summary>
        /// Deactivate state
        /// </summary>
        public void TurnOff()
        {
            SetValue(false);
        }
    }
}
