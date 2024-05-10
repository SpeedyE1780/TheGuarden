using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    public class MushroomUnlockedWindow : MonoBehaviour
    {
        [SerializeField, Tooltip("Window")]
        private RectTransform window;
        [SerializeField, Tooltip("Mushroom Name")]
        private TextMeshProUGUI mushroomName;
        [SerializeField, Tooltip("Mushroom Description")]
        private TextMeshProUGUI mushroomDescription;
        [SerializeField]
        private Image mushroomIcon;
        [SerializeField]
        private GameEvent mushroomWindowActive;

        private List<MushroomInfo> mushroomList = new List<MushroomInfo>();
        private HashSet<MushroomInfo> unlockedMushrooms = new HashSet<MushroomInfo>();
        private bool hideWindow = false;

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

        public void ShowMushroomTutorial(MushroomInfo mushroom)
        {
            if (mushroomList.Contains(mushroom))
            {
                StartCoroutine(PopUp(mushroom));
            }
        }

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

        public void HideWindow()
        {
            hideWindow = true;
        }
    }
}
