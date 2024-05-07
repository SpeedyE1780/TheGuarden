using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Utility
{
    public class GameState : MonoBehaviour
    {
        [SerializeField]
        private GameEvent onGameStarted;

        private void Start()
        {
            onGameStarted.Raise();
        }
    }
}
