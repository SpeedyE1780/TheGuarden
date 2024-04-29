using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility
{
    /// <summary>
    /// DayLightCycle rotates the directional light to give a day and night cycle
    /// </summary>
    internal class DayLightCycle : MonoBehaviour
    {
        [SerializeField, Tooltip("Day Duration")]
        private float dayDuration = 180.0f;
        [SerializeField, Tooltip("Day Night Transition Duration")]
        private float transitionDuration = 3.0f;
        [SerializeField, Tooltip("Angle Curve")]
        internal AnimationCurve curve;

        public UnityEvent OnDayStarted;
        public UnityEvent OnNightStarted;

        private void Start()
        {
            StartCoroutine(RunCycle());
        }

        internal void UpdateLight(float progress)
        {
            float angle = curve.Evaluate(progress);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private IEnumerator UpdateLight(int offset = 0)
        {
            float time = 0;
            while (time < transitionDuration)
            {
                time += Time.deltaTime;
                UpdateLight(offset + time / transitionDuration);
                yield return null;
            }
        }

        private IEnumerator RunCycle()
        {
            while (true)
            {
                OnDayStarted.Invoke();
                GameLogger.LogInfo("Day Started", this, GameLogger.LogCategory.Scene);
                yield return new WaitForSeconds(dayDuration);

                yield return UpdateLight();

                OnNightStarted.Invoke();
                GameLogger.LogInfo("Night Started", this, GameLogger.LogCategory.Scene);

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Numlock));

                yield return UpdateLight(1);
            }
        }
    }
}
