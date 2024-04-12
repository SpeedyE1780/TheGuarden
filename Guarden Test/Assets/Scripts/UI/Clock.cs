using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameTime gameTime;
    [SerializeField] GameObject minuteHand, hourHand;
    [SerializeField] TMP_Text dateText;
    float targetMinuteAngle;
    float targetHourAngle;
    const float smoothSpeed = 5f; // Adjust the speed of rotation

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

    private void OnValidate()
    {
        gameTime = FindObjectOfType<GameTime>();

        if (gameTime == null)
        {
            GameLogger.LogWarning("Game Time not available in scene", gameObject);
        }
    }
}
