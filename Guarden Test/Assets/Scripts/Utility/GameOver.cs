using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Waits until game ends and show game over menu
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        [SerializeField, Tooltip("Game Over menu")]
        private GameObject gameOverMenu;

        /// <summary>
        /// Show game over and pause game
        /// </summary>
        public void OnGameEnded()
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
