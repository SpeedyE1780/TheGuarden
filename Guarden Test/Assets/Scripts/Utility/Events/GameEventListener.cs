using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameEvent gameEvent;

        public UnityEvent Response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        internal void OnEventRaised()
        {
            Response.Invoke();
        }
    }

    public class TGameEventListener<T> : MonoBehaviour
    {
        [SerializeField]
        private TGameEvent<T> gameEvent;

        public UnityEvent<T> Response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        internal void OnEventRaised(T arg)
        {
            Response.Invoke(arg);
        }
    }
}
