using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility.Events
{
    /// <summary>
    /// SO that will be called when game event is raised
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Listeners/<void>")]
    public class GameEventListenerSO : ScriptableObject
    {
        public UnityEvent Response;

        internal void OnEventRaised()
        {
            Response.Invoke();
        }
    }

    /// <summary>
    /// SO that will be called when game event is raised
    /// </summary>
    /// <typeparam name="T">Argument that event passes in</typeparam>
    public class TGameEventListenerSO<T> : ScriptableObject
    {
        public UnityEvent<T> Response;

        internal void OnEventRaised(T arg)
        {
            Response.Invoke(arg);
        }
    }
}
