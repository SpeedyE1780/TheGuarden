using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Game Event<void>")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField]
        private GameEventListenerSO soListeners;

        private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnEventRaised();
            }

            soListeners?.OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}
