using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility.Events
{
    /// <summary>
    /// Listens to a GameEvent and invoke response
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        private enum RegisterTime
        {
            Enable_Disable = 0,
            Awake_Destroy = 1,
            Manul = 2
        }

        [SerializeField, Tooltip("Game event listening to")]
        private GameEvent gameEvent;
        [SerializeField, Tooltip("When to register/unregister to event")]
        private RegisterTime registerTime;

        public UnityEvent Response;

        private void Awake()
        {
            if (registerTime == RegisterTime.Awake_Destroy)
            {
                gameEvent.RegisterListener(this);
            }
        }

        private void OnEnable()
        {
            if (registerTime == RegisterTime.Enable_Disable)
            {
                gameEvent.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if (registerTime == RegisterTime.Enable_Disable)
            {
                gameEvent.UnregisterListener(this);
            }
        }

        private void OnDestroy()
        {
            if (registerTime == RegisterTime.Awake_Destroy)
            {
                gameEvent.UnregisterListener(this);
            }
        }

        /// <summary>
        /// Invokes unity event when event is raised
        /// </summary>
        internal void OnEventRaised()
        {
            Response.Invoke();
        }
    }

    /// <summary>
    /// Listens to a GameEvent<T> and invoke response
    /// </summary>
    /// <typeparam name="T">Argument passed in game event</typeparam>
    public class TGameEventListener<T> : MonoBehaviour
    {
        private enum RegisterTime
        {
            Enable_Disable = 0,
            Awake_Destroy = 1,
            Manul = 2
        }

        [SerializeField, Tooltip("Game event listening to")]
        private TGameEvent<T> gameEvent;
        [SerializeField, Tooltip("When to register/unregister to event")]
        private RegisterTime registerTime;

        public UnityEvent<T> Response;

        private void Awake()
        {
            if (registerTime == RegisterTime.Awake_Destroy)
            {
                gameEvent.RegisterListener(this);
            }
        }

        private void OnEnable()
        {
            if (registerTime == RegisterTime.Enable_Disable)
            {
                gameEvent.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if (registerTime == RegisterTime.Enable_Disable)
            {
                gameEvent.UnregisterListener(this);
            }
        }

        private void OnDestroy()
        {
            if (registerTime == RegisterTime.Awake_Destroy)
            {
                gameEvent.UnregisterListener(this);
            }
        }

        /// <summary>
        /// Invokes unity event when event is raised
        /// </summary>
        internal void OnEventRaised(T arg)
        {
            Response.Invoke(arg);
        }
    }
}
