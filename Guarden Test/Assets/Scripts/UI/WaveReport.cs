using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using TMPro;
using UnityEngine;

namespace TheGuarden.UI
{
    public class WaveReport : MonoBehaviour
    {
        [SerializeField]
        private GameObject reportWindow;
        [SerializeField]
        private TextMeshProUGUI cowsKidnappedText;
        [SerializeField]
        private TextMeshProUGUI enemiesKilledText;
        [SerializeField]
        private GameEvent onShowInstructions;

        int cowsKidnapped = 0;
        int enemiesKilled = 0;

        public void OnNightStarted()
        {
            enemiesKilled = 0;
            cowsKidnapped = 0;
        }

        public void OnEnemyKilled()
        {
            enemiesKilled += 1;
        }

        public void OnCowKidnapped()
        {
            cowsKidnapped += 1;
        }

        public void OnWaveEnded()
        {
            GameLogger.LogInfo("Show Wave Report", this, GameLogger.LogCategory.UI);
            onShowInstructions.Raise();
            Time.timeScale = 0.0f;
            cowsKidnappedText.text = $"Cows Kidnapped: {cowsKidnapped}";
            enemiesKilledText.text = $"Enemies Killed: {enemiesKilled}";
            reportWindow.SetActive(true);
        }

        public void OnHideWindow()
        {
            GameLogger.LogInfo("Hide Wave Report", this, GameLogger.LogCategory.UI);
            Time.timeScale = 1.0f;
            reportWindow.SetActive(false);
        }
    }
}
