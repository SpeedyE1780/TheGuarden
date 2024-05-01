using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility.Events
{
    internal class GameEventListener : MonoBehaviour
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
}
