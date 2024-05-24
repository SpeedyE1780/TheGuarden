using UnityEngine;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Hide Instructions")]
    internal class HideTutorialInstruction : ScriptableObject
    {
        public delegate void Hide();
        public static event Hide OnHide;

        public void RaiseHide()
        {
            OnHide?.Invoke();
        }
    }
}
