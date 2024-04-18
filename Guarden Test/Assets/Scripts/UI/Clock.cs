using TMPro;
using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.UI
{
    /// <summary>
    /// Clock represents a UI analog clock on the canvas
    /// </summary>
    internal class Clock : MonoBehaviour
    {
        [SerializeField, Tooltip("GameTime in scene")]
        private GameTime gameTime;
        [SerializeField, Tooltip("Minute hand of clock")]
        private GameObject minuteHand;
        [SerializeField, Tooltip("Hour hand of clock")]
        private GameObject hourHand;
        [SerializeField, Tooltip("Date text on screen")]
        private TMP_Text dateText;

        private float targetMinuteAngle;
        private float targetHourAngle;
        private const float smoothSpeed = 5f; // Adjust the speed of rotation

        void Update()
        {
            // Calculate the target angles
            float currentMinuteAngle = -minuteHand.transform.localEulerAngles.z;
            float currentHourAngle = -hourHand.transform.localEulerAngles.z;
            targetMinuteAngle = 360f * (gameTime.Minute / 60f);
            targetHourAngle = 360f * (gameTime.Hour % 12) / 12f + (gameTime.Minute / 60f) * 30f; // Include minute contribution

            // Smoothly interpolate between current and target angles
            float newMinuteAngle = Mathf.LerpAngle(currentMinuteAngle, targetMinuteAngle, smoothSpeed * Time.deltaTime);
            float newHourAngle = Mathf.LerpAngle(currentHourAngle, targetHourAngle, smoothSpeed * Time.deltaTime);

            // Apply the new angles to clock hands
            minuteHand.transform.localRotation = Quaternion.Euler(0, 0, -newMinuteAngle);
            hourHand.transform.localRotation = Quaternion.Euler(0, 0, -newHourAngle);

            dateText.text = gameTime.DateText;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("Game Time not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
