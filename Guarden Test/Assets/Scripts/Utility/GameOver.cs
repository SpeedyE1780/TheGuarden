using UnityEngine;

namespace TheGuarden.Utility
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameOverMenu;

        public void OnGameEnded()
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
