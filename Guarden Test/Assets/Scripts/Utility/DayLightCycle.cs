using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility
{
    /// <summary>
    /// DayLightCycle rotates the directional light to give a day and night cycle
    /// </summary>
    public class DayLightCycle : MonoBehaviour
    {
        public delegate void DayStarted();
        public delegate void NightStarted();
        public delegate void WaveEnded();

        [SerializeField, Tooltip("Day Duration")]
        private float dayDuration = 180.0f;
        [SerializeField, Tooltip("Day Night Transition Duration")]
        private float transitionDuration = 3.0f;
        [SerializeField, Tooltip("Angle Curve")]
        internal AnimationCurve curve;

        private bool enemyWavedEnded = false;
        public UnityEvent OnDayStartedEvent;
        public UnityEvent OnNightStartedEvent;

        public static event DayStarted OnDayStarted;
        public static event NightStarted OnNightStarted;
        public static event WaveEnded OnWaveEnded;

        public static void EnemyWaveEnded()
        {
            OnWaveEnded?.Invoke();
        }

        private void Start()
        {
            StartCoroutine(RunCycle());
        }

        private void OnEnable()
        {
            OnWaveEnded += OnEnemyWaveEnded;
            OnDayStarted += OnDayStartedEvent.Invoke;
            OnNightStarted += OnNightStartedEvent.Invoke;
        }

        private void OnDisable()
        {
            OnWaveEnded -= OnEnemyWaveEnded;
            OnDayStarted -= OnDayStartedEvent.Invoke;
            OnNightStarted -= OnNightStartedEvent.Invoke;
        }

        private void OnEnemyWaveEnded()
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
            while (true)
            {
                OnDayStarted.Invoke();
                GameLogger.LogInfo("Day Started", this, GameLogger.LogCategory.Scene);
                yield return new WaitForSeconds(dayDuration);

                yield return UpdateLight();

                enemyWavedEnded = false;
                OnNightStarted.Invoke();
                GameLogger.LogInfo("Night Started", this, GameLogger.LogCategory.Scene);

                yield return new WaitUntil(() => enemyWavedEnded);

                yield return UpdateLight(1);
            }
        }
    }
}
