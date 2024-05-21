using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.VFX;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// EnemySpawner spawns enemy waves
    /// </summary>
    internal class EnemySpawner : MonoBehaviour
    {
        /// <summary>
        /// Class representing how long each wave lasts
        /// </summary>
        [System.Serializable]
        private class WaveConfig
        {
            [SerializeField]
            internal int days;
            [SerializeField]
            internal Wave wave;
        }

        [SerializeField, Tooltip("Position where enemies will be spawned")]
        private Transform spawnPoint;
        [SerializeField, Tooltip("List of paths enemies can take")]
        private List<EnemyPath> paths;
        [SerializeField, Tooltip("Autofilled. Camera following players")]
        private FollowTarget followCamera;
        [SerializeField, Tooltip("UFO transform that will show up when spawning enemies")]
        private Transform ufoTransform;
        [SerializeField, Tooltip("UFO Visual effect that will drop enemy")]
        private VisualEffect ufo;
        [SerializeField, Tooltip("UFO movement speed")]
        private float ufoSpeed = 75.0f;
        [SerializeField, Tooltip("Wave of enemies spawned")]
        private List<WaveConfig> waveConfigs;
        [SerializeField, Tooltip("List of all enemies in scene")]
        private EnemySet enemySet;
        [SerializeField, Tooltip("Game event raised when all enemies are disabled")]
        private GameEvent onWaveEnded;
        [SerializeField, Tooltip("Multiplier added each night to enemy's max health")]
        private float healthMultiplier = 0.5f;
        [SerializeField, Tooltip("VFX OnSucking Property")]
        private ExposedProperty onSucking;
        [SerializeField, Tooltip("VFX OnFinishSucking Property")]
        private ExposedProperty onFinishSucking;

        [SerializeField, Tooltip("Audio Source")]
        private AudioSource audioSource;
        [SerializeField,Tooltip("Hover Sound")]
        private AudioClip hoverClip;
        [SerializeField, Tooltip("Enter Sound")]
        private AudioClip UFOEnterClip;

        private float currentHealthMultiplier = 1.0f;
        private int currentWaveConfig = 0;

        private WaveConfig CurrentWave => waveConfigs[currentWaveConfig];

#if UNITY_EDITOR
        internal List<EnemyPath> Paths => paths;
#endif

        /// <summary>
        /// Calleld when NightStarted event is called
        /// </summary>
        public void StartSpawning()
        {
            StartCoroutine(SpawnEnemies());
            currentHealthMultiplier += healthMultiplier;
        }

        /// <summary>
        /// Move UFO to target position
        /// </summary>
        /// <param name="targetPosition">Target UFO position</param>
        /// <returns></returns>
        private IEnumerator MoveUFO(Vector3 targetPosition)
        {
            //Keep UFO at same height
            targetPosition.y = ufoTransform.position.y;

            while (ufoTransform.position != targetPosition)
            {
                ufoTransform.position = Vector3.MoveTowards(ufoTransform.position, targetPosition, Time.deltaTime * ufoSpeed);
                yield return null;
            }
        }

        /// <summary>
        /// Check if spawner should switch to next wave configuration
        /// </summary>
        private void UpdateCurrentWave()
        {
            CurrentWave.days -= 1;

            if (CurrentWave.days <= 0 && currentWaveConfig < waveConfigs.Count - 1)
            {
                currentWaveConfig += 1;
                GameLogger.LogInfo($"{name} switching to {CurrentWave.wave.name} wave", this, GameLogger.LogCategory.Enemy);
            }
        }

        /// <summary>
        /// Spawn Enemies
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnEnemies()
        {
            Vector3 startPosition = ufoTransform.position;
            Vector3 endPosition = spawnPoint.position - startPosition;
            ufoTransform.gameObject.SetActive(true);
            followCamera.AddTarget(ufoTransform);

            yield return MoveUFO(spawnPoint.position);
            ufo.Play();
            audioSource.Play();
            ufo.SendEvent(onSucking.PropertyID);

            SpawnConfiguration configuration = new SpawnConfiguration()
            {
                paths = paths,
                position = spawnPoint.position,
                rotation = spawnPoint.rotation,
                healthMultiplier = currentHealthMultiplier,
            };

            yield return CurrentWave.wave.SpawnWave(configuration);

            ufo.SendEvent(onFinishSucking.PropertyID);
            followCamera.RemoveTarget(ufoTransform);
            yield return MoveUFO(endPosition);

            ufo.Stop();
            audioSource.Stop();
            ufoTransform.gameObject.SetActive(false);

            yield return new WaitUntil(() => enemySet.Count == 0);

            UpdateCurrentWave();
            onWaveEnded.Raise();
        }

#if UNITY_EDITOR
        internal void AutofillVariables()
        {
            followCamera = FindObjectOfType<FollowTarget>();

            if (followCamera == null)
            {
                GameLogger.LogWarning("Follow Camera not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
