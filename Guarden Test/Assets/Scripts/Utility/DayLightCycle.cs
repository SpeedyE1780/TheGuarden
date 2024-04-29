using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// DayLightCycle rotates the directional light to give a day and night cycle
    /// </summary>
    internal class DayLightCycle : MonoBehaviour
    {
        [SerializeField, Tooltip("Angle Curve")]
        internal AnimationCurve curve;

        internal void UpdateLight(float progress)
        {
            float angle = curve.Evaluate(progress);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        void LateUpdate()
        {
            UpdateLight(GameTime.DayEndProgress);
        }
    }
}
