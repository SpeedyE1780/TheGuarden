using System.Collections;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// GameState controls the state of the game on sends events accordingly
    /// </summary>
    public class GameState : MonoBehaviour
    {
        [SerializeField, Tooltip("Game Loaded event")]
        private GameEvent onGameLoaded;
        [SerializeField, Tooltip("Game Started event")]
        private GameEvent onGameStarted;
        [SerializeField, Tooltip("Exit Game event")]
        private GameEvent onExitGame;
        [SerializeField, Tooltip("State indicating if player joined the game")]
        private StateToggle playerState;

        private void Awake()
        {
            onGameLoaded.Raise();
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => playerState.Toggled);
            onGameStarted.Raise();
        }

        private void OnDestroy()
        {
            onExitGame.Raise();
        }
    }
}
