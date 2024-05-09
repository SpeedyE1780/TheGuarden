using System.Collections;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Utility
{
    public class GameState : MonoBehaviour
    {
        [SerializeField]
        private GameEvent onGameStarted;
        [SerializeField]
        private GameEvent onExitGame;
        [SerializeField]
        private StateToggle playerState;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => playerState.Toggled);
            onGameStarted.Raise();
        }

        private void OnDestroy()
        {

        }
    }
}
