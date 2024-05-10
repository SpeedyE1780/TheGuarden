using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility.Events
{
    /// <summary>
    /// GameEvent will notify all listeners once raised
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Events/<void>")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField, Tooltip("SO listener")]
        private GameEventListenerSO soListeners;

        private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();

        /// <summary>
        /// Notify listener that event is raised
        /// </summary>
        public void Raise()
        {
            if (soListeners != null)
            {
                soListeners.OnEventRaised();
            }

            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnEventRaised();
            }
        }

        /// <summary>
        /// Register listener to event
        /// </summary>
        /// <param name="listener">Listener listening to event</param>
        public void RegisterListener(GameEventListener listener)
        {
            eventListeners.SafeAdd(listener);
        }

        /// <summary>
        /// Unregister listener to event
        /// </summary>
        /// <param name="listener">listener no longer listening to event</param>
        public void UnregisterListener(GameEventListener listener)
        {
            eventListeners.Remove(listener);
        }
    }

    /// <summary>
    /// GameEvent will notify all listeners once raised
    /// </summary>
    /// <typeparam name="T">Argument passed in event</typeparam>
    public class TGameEvent<T> : ScriptableObject
    {
        [SerializeField]
        private TGameEventListenerSO<T> soListeners;

        private readonly List<TGameEventListener<T>> eventListeners = new List<TGameEventListener<T>>();

        /// <summary>
        /// Notify listener that event is raised
        /// </summary>
        /// <param name="arg">Parameter passed in to listeners</param>
        public void Raise(T arg)
        {
            if (soListeners != null)
            {
                soListeners.OnEventRaised(arg);
            }

            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnEventRaised(arg);
            }
        }

        /// <summary>
        /// Register listener to event
        /// </summary>
        /// <param name="listener">Listener listening to event</param>
        public void RegisterListener(TGameEventListener<T> listener)
        {
            eventListeners.SafeAdd(listener);
        }

        /// <summary>
        /// Unregister listener to event
        /// </summary>
        /// <param name="listener">listener no longer listening to event</param>
        public void UnregisterListener(TGameEventListener<T> listener)
        {
            eventListeners.Remove(listener);
        }
    }
}
