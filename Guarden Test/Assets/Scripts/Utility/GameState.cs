using System.Collections;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Utility
{
    public class GameState : MonoBehaviour
    {
        [SerializeField]
        private GameEvent onGameLoaded;
        [SerializeField]
        private GameEvent onGameStarted;
        [SerializeField]
        private GameEvent onExitGame;
        [SerializeField]
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
