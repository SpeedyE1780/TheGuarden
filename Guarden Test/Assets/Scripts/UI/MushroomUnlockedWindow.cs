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
        private StateToggle playersInScene;
        [SerializeField]
        private GameEvent mushroomWindowActive;

        private List<MushroomInfo> mushroomList = new List<MushroomInfo>();
        private HashSet<MushroomInfo> unlockedMushrooms = new HashSet<MushroomInfo>();
        private bool hideWindow = false;

        private void Start()
        {
            StartCoroutine(WaitForMushroomUnlocked());
        }

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

        public void HideWindow()
        {
            hideWindow = true;
        }

        private IEnumerator WaitForMushroomUnlocked()
        {
            while (true)
            {
                yield return new WaitUntil(() => mushroomList.Count > 0);

                while (mushroomList.Count > 0)
                {
                    yield return new WaitUntil(() => playersInScene.Toggled);
                    yield return PopUp(mushroomList[0]);
                }
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

            if (mushroomList.Count == 0)
            {
                window.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
