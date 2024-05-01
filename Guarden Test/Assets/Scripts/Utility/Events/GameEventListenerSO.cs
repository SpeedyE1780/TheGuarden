using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Game Event Listener")]
    public class GameEventListenerSO : ScriptableObject
    {
        public UnityEvent Response;

        internal void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}
