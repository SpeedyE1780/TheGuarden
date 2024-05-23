using System.Collections;
using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private IEnumerator Start()
        {
            onGameLoaded.Raise();
            yield return new WaitUntil(() => playerState.Value);
            onGameStarted.Raise();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnDestroy()
        {
            onExitGame.Raise();
        }
    }
}
