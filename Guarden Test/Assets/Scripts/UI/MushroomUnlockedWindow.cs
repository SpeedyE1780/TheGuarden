using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    /// <summary>
    /// Mushroom tutorial window explaining what the mushroom does
    /// </summary>
    public class MushroomUnlockedWindow : MonoBehaviour
    {
        [SerializeField, Tooltip("Window")]
        private RectTransform window;
        [SerializeField, Tooltip("Mushroom Name")]
        private TextMeshProUGUI mushroomName;
        [SerializeField, Tooltip("Mushroom Description")]
        private TextMeshProUGUI mushroomDescription;
        [SerializeField, Tooltip("Mushroom Icon")]
        private Image mushroomIcon;
        [SerializeField, Tooltip("Game Event raised when window is active")]
        private GameEvent mushroomWindowActive;

        private List<MushroomInfo> mushroomList = new List<MushroomInfo>();
        private HashSet<MushroomInfo> unlockedMushrooms = new HashSet<MushroomInfo>();
        private bool hideWindow = false;

        /// <summary>
        /// Called when new mushroom is unlocked
        /// </summary>
        /// <param name="mushroom">Unlocked mushroom info</param>
        public void OnMushroomUnlocked(MushroomInfo mushroom)
        {
            if (!unlockedMushrooms.Contains(mushroom))
            {
                unlockedMushrooms.Add(mushroom);
                mushroomList.Add(mushroom);
            }
            else
            {
                GameLogger.LogWarning($"{mushroom.Name} already added in list", this, GameLogger.LogCategory.PlantPowerUp);
            }
        }

        /// <summary>
        /// Called when mushroom is picked up
        /// </summary>
        /// <param name="mushroom">Picked up mushroom info</param>
        public void ShowMushroomTutorial(MushroomInfo mushroom)
        {
            if (mushroomList.Contains(mushroom))
            {
                StartCoroutine(PopUp(mushroom));
            }
        }

        /// <summary>
        /// Pause game and show mushroom tutorial
        /// </summary>
        /// <param name="mushroom">Mushroom info shown in tutorial</param>
        /// <returns></returns>
        private IEnumerator PopUp(MushroomInfo mushroom)
        {
            mushroomWindowActive.Raise();
            mushroomName.text = mushroom.Name;
            mushroomDescription.text = mushroom.Description;
            mushroomIcon.sprite = mushroom.Sprite;
            window.gameObject.SetActive(true);
            Time.timeScale = 0;
            hideWindow = false;
            yield return new WaitUntil(() => hideWindow);
            mushroomList.Remove(mushroom);
            window.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        /// <summary>
        /// Called when hide input is pressed
        /// </summary>
        public void HideWindow()
        {
            hideWindow = true;
        }
    }
}
