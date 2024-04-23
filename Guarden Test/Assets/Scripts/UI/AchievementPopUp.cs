using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TheGuarden.Achievements;

namespace TheGuarden.UI
{
    internal class AchievementPopUp : MonoBehaviour
    {
        [SerializeField, Tooltip("PopUpWindow")]
        private RectTransform popUpWindow;
        [SerializeField, Tooltip("Achievement Name")]
        private TextMeshProUGUI achievementName;
        [SerializeField, Tooltip("Pop up speed")]
        private float speed = 200.0f;
        [SerializeField, Tooltip("Pop up duration on screen")]
        private float duration = 2.0f;
        [SerializeField, Tooltip("Delay between each pop up")]
        private float delay = 1.0f;

        private List<Achievement> achievementList = new List<Achievement>();

        private void Start()
        {
            StartCoroutine(WaitForAchievement());
        }

        /// <summary>
        /// Called from AchievementManager event
        /// </summary>
        /// <param name="achievement">Completed Achievement</param>
        public void OnAchievementCompleted(Achievement achievement)
        {
            achievementList.Add(achievement);
        }

        /// <summary>
        /// Enable queueing completed achievement without running multiple pop up coroutine simultaneously
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForAchievement()
        {
            while (true)
            {
                yield return new WaitUntil(() => achievementList.Count > 0);

                while (achievementList.Count > 0)
                {
                    yield return PopUp(achievementList[0]);
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        /// <summary>
        /// Pop up achievement window
        /// </summary>
        /// <returns></returns>
        private IEnumerator PopUp(Achievement achievement)
        {
            achievementName.text = achievement.name;
            popUpWindow.gameObject.SetActive(true);
            Vector2 startPosition = popUpWindow.anchoredPosition;

            while (popUpWindow.anchoredPosition != Vector2.zero)
            {
                popUpWindow.anchoredPosition = Vector2.MoveTowards(popUpWindow.anchoredPosition, Vector2.zero, speed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(duration);

            while (popUpWindow.anchoredPosition != startPosition)
            {
                popUpWindow.anchoredPosition = Vector2.MoveTowards(popUpWindow.anchoredPosition, startPosition, speed * Time.deltaTime);
                yield return null;
            }

            popUpWindow.gameObject.SetActive(false);
            achievementList.Remove(achievement);
        }
    }
}
