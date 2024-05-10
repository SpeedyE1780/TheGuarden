using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Used to keep track of a game state
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/State Toggle")]
    public class StateToggle : ScriptableObject
    {
        public delegate void ValueChange();
        public event ValueChange OnValueChange;

        public bool Toggled { get; private set; }

        /// <summary>
        /// Activate state
        /// </summary>
        public void TurnOn()
        {
            Toggled = true;
            OnValueChange?.Invoke();
        }

        /// <summary>
        /// Deactivate state
        /// </summary>
        public void TurnOff()
        {
            Toggled = false;
            OnValueChange?.Invoke();
        }
    }
}
