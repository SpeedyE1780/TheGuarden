using System.Collections;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// DayLightCycle rotates the directional light to give a day and night cycle
    /// </summary>
    public class DayLightCycle : MonoBehaviour
    {
        [SerializeField, Tooltip("Day Duration")]
        private float dayDuration = 180.0f;
        [SerializeField, Tooltip("Day Night Transition Duration")]
        private float transitionDuration = 3.0f;
        [SerializeField, Tooltip("Angle Curve")]
        internal AnimationCurve curve;
        [SerializeField]
        private GameEvent onDayStarted;
        [SerializeField]
        private GameEvent onNightStarted;
        private bool enemyWavedEnded = false;

        private void Start()
        {
            StartCoroutine(RunCycle());
        }

        public void OnEnemyWaveEnded()
        {
            enemyWavedEnded = true;
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
            //Wait one frame so all objects start passed
            yield return null;

            while (true)
            {
                onDayStarted.Raise();
                GameLogger.LogInfo("Day Started", this, GameLogger.LogCategory.Scene);
                yield return new WaitForSeconds(dayDuration);
                yield return UpdateLight();

                enemyWavedEnded = false;
                onNightStarted.Raise();
                GameLogger.LogInfo("Night Started", this, GameLogger.LogCategory.Scene);

                yield return new WaitUntil(() => enemyWavedEnded);
                yield return UpdateLight(1);
            }
        }
    }
}
