using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Events/<void>")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField]
        private GameEventListenerSO soListeners;

        private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();

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

        public void RegisterListener(GameEventListener listener)
        {
            eventListeners.SafeAdd(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            eventListeners.Remove(listener);
        }
    }

    public class TGameEvent<T> : ScriptableObject
    {
        [SerializeField]
        private TGameEventListenerSO<T> soListeners;

        private readonly List<TGameEventListener<T>> eventListeners = new List<TGameEventListener<T>>();

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

        public void RegisterListener(TGameEventListener<T> listener)
        {
            eventListeners.SafeAdd(listener);
        }

        public void UnregisterListener(TGameEventListener<T> listener)
        {
            eventListeners.Remove(listener);
        }
    }
}
