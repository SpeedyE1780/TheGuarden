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

        /// <summary>
        /// Called from AchievementManager event
        /// </summary>
        /// <param name="achievement">Completed Achievement</param>
        public void OnAchievementCompleted(Achievement achievement)
        {
            achievementName.text = achievement.name;
            StartCoroutine(Popup());
        }

        /// <summary>
        /// Pop up achievement window
        /// </summary>
        /// <returns></returns>
        private IEnumerator Popup()
        {
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
        }
    }
}
