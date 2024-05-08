using UnityEngine;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/State Toggle")]
    public class StateToggle : ScriptableObject
    {
        public delegate void ValueChange();
        public event ValueChange OnValueChange;

        public bool Toggled { get; private set; }

        public void TurnOn()
        {
            Toggled = true;
            OnValueChange?.Invoke();
        }

        public void TurnOff()
        {
            Toggled = false;
            OnValueChange?.Invoke();
        }
    }
}
