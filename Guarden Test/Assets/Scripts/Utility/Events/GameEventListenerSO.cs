using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Game Event Listener<void>")]
    public class GameEventListenerSO : ScriptableObject
    {
        public UnityEvent Response;

        internal void OnEventRaised()
        {
            Response.Invoke();
        }
    }

    public class TGameEventListenerSO<T> : ScriptableObject
    {
        public UnityEvent<T> Response;

        internal void OnEventRaised(T arg)
        {
            Response.Invoke(arg);
        }
    }
}
