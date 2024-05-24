using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using TMPro;
using UnityEngine;

namespace TheGuarden.UI
{
    /// <summary>
    /// WaveReport showing wave stats
    /// </summary>
    public class WaveReport : MonoBehaviour
    {
        [SerializeField, Tooltip("Report window")]
        private GameObject reportWindow;
        [SerializeField, Tooltip("Cows kidnapped stat text")]
        private TextMeshProUGUI cowsKidnappedText;
        [SerializeField, Tooltip("Enemies killed stat text")]
        private TextMeshProUGUI enemiesKilledText;
        [SerializeField, Tooltip("Game Event raised when window is active")]
        private GameEvent onWindowActive;

        int cowsKidnapped = 0;
        int enemiesKilled = 0;

        /// <summary>
        /// Reset wave stats
        /// </summary>
        public void OnNightStarted()
        {
            enemiesKilled = 0;
            cowsKidnapped = 0;
        }

        /// <summary>
        /// Increment enemies killed
        /// </summary>
        public void OnEnemyKilled()
        {
            enemiesKilled += 1;
        }

        /// <summary>
        /// Increment cows kidnapped
        /// </summary>
        public void OnCowKidnapped()
        {
            cowsKidnapped += 1;
        }

        /// <summary>
        /// Show report when wave ends and pause game
        /// </summary>
        public void OnWaveEnded()
        {
            GameLogger.LogInfo("Show Wave Report", this, GameLogger.LogCategory.UI);
            onWindowActive.Raise();
            Time.timeScale = 0.0f;
            cowsKidnappedText.text = $"Animals Kidnapped: {cowsKidnapped}";
            enemiesKilledText.text = $"Enemies Killed: {enemiesKilled}";
            reportWindow.SetActive(true);
        }

        /// <summary>
        /// Hide window when active and resume game
        /// </summary>
        public void OnHideWindow()
        {
            if (reportWindow.activeSelf)
            {
                GameLogger.LogInfo("Hide Wave Report", this, GameLogger.LogCategory.UI);
                Time.timeScale = 1.0f;
                reportWindow.SetActive(false);
            }
        }
    }
}
