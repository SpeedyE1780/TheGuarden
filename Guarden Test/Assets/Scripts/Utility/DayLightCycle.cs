using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// DayLightCycle rotates the directional light to give a day and night cycle
    /// </summary>
    public class DayLightCycle : MonoBehaviour
    {
        void LateUpdate()
        {
            float zAngle = Mathf.Lerp(180, -180, GameTime.DayEndProgress);
            transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }
    }
}
