using UnityEngine;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/VFX Play State")]
    internal class VFXPlayState : ScriptableObject
    {
        internal delegate void OnValueChange();
        public event OnValueChange onValueChange;

        public bool PlayOnEnable { get; private set; }

        public void Play()
        {
            PlayOnEnable = true;
            onValueChange?.Invoke();
        }

        public void Stop()
        {
            PlayOnEnable = false;
            onValueChange?.Invoke();
        }
    }
}
