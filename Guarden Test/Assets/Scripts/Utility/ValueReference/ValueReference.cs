using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// ValueReference allows multiple classes to modify and read the same value
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    public class ValueReference<T> : ScriptableObject
    {
        public delegate void ValueChange(T value);
        public event ValueChange OnValueChange;

        public T Value { get; private set; }

        public void SetValue(T value)
        {
            if (Equals(Value, value))
            {
                return;
            }

            Value = value;
            OnValueChange?.Invoke(Value);
        }
    }
}
