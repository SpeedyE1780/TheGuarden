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
        [SerializeField, Tooltip("Day Started event")]
        private GameEvent onDayStarted;
        [SerializeField, Tooltip("Night Started event")]
        private GameEvent onNightStarted;
        [SerializeField, Tooltip("Background Music Audio Source")]
        private AudioSource bgAudioSource;
        [SerializeField, Tooltip("Day Time clip")]
        private AudioClip dayClip;
        [SerializeField, Tooltip("Night Time clip")]
        private AudioClip nightClip;

        private float dayTime = 0;
        private bool enemyWavedEnded = false;
        private bool startWaveEarly = false;

        public float DayProgess => dayTime / dayDuration;

        private void Start()
        {
            ChangeAudioClip(dayClip);
        }

        private void ChangeAudioClip(AudioClip clip)
        {
            if (bgAudioSource.clip != clip || !bgAudioSource.isPlaying)
            {
                bgAudioSource.clip = clip;
                bgAudioSource.Play();
            }
        }

        /// <summary>
        /// Start the daylight cycle when game starts
        /// </summary>
        public void OnGameStarted()
        {
            StartCoroutine(RunCycle());
        }

        /// <summary>
        /// Called from game event
        /// </summary>
        public void OnEnemyWaveEnded()
        {
            enemyWavedEnded = true;
        }

        /// <summary>
        /// Called from game event
        /// </summary>
        public void OnStartWaveEarly()
        {
            startWaveEarly = true;
        }

        /// <summary>
        /// Update light rotation
        /// </summary>
        /// <param name="progress">time value in curve</param>
        internal void UpdateLight(float progress)
        {
            float angle = curve.Evaluate(progress);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        /// <summary>
        /// Transition light from day/night to night/day
        /// </summary>
        /// <param name="offset">0 to transition from day/night 1 to transtion from night/day</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update daytime until it reaches duration and start night
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunDay()
        {
            GameLogger.LogInfo("Day Started", this, GameLogger.LogCategory.Scene);
            ChangeAudioClip(dayClip);
            dayTime = 0;
            startWaveEarly = false;

            while (dayTime < dayDuration && !startWaveEarly)
            {
                dayTime += Time.deltaTime;
                yield return null;
            }

            dayTime = dayDuration;
            yield return UpdateLight();
        }

        /// <summary>
        /// Start enemy wave and wait until all enemies are gone
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunNight()
        {
            GameLogger.LogInfo("Night Started", this, GameLogger.LogCategory.Scene);
            ChangeAudioClip(nightClip);
            enemyWavedEnded = false;
            onNightStarted.Raise();

            yield return new WaitUntil(() => enemyWavedEnded);
            yield return UpdateLight(1);
        }

        /// <summary>
        /// Transition from daytime to night time
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunCycle()
        {
            while (true)
            {
                yield return RunDay();
                yield return RunNight();
                onDayStarted.Raise();
            }
        }
    }
}
