using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] GameObject minuteHand, hourHand;
    // Start is called before the first frame update
    float targetMinuteAngle;
    float targetHourAngle;
    const float smoothSpeed = 5f; // Adjust the speed of rotation

    void Update()
    {
        // Calculate the target angles
        float currentMinuteAngle = -minuteHand.transform.localEulerAngles.z;
        float currentHourAngle = -hourHand.transform.localEulerAngles.z;
        targetMinuteAngle = 360f * (GameTime.minute / 60f);
        targetHourAngle = 360f * (GameTime.hour % 12) / 12f + (GameTime.minute / 60f) * 30f; // Include minute contribution

        // Smoothly interpolate between current and target angles
        float newMinuteAngle = Mathf.LerpAngle(currentMinuteAngle, targetMinuteAngle, smoothSpeed * Time.deltaTime);
        float newHourAngle = Mathf.LerpAngle(currentHourAngle, targetHourAngle, smoothSpeed * Time.deltaTime);

        // Apply the new angles to clock hands
        minuteHand.transform.localRotation = Quaternion.Euler(0, 0, -newMinuteAngle);
        hourHand.transform.localRotation = Quaternion.Euler(0, 0, -newHourAngle);
    }
}
