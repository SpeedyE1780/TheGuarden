using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine.VFX;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// EnemySpawner spawns enemy during time period
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
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
        [SerializeField, Tooltip("UFO transform that moves it in the scene")]
        private Transform ufoTransform;
        [SerializeField, Tooltip("UFO Visual effect that will drop enemy")]
        private VisualEffect ufo;
        [SerializeField, Tooltip("UFO movement speed")]
        private float ufoSpeed = 75.0f;
        [SerializeField, Tooltip("Wave of enemies spawned")]
        private List<WaveConfig> waveConfigs;
        [SerializeField, Tooltip("List of all enemies in scene")]
        private EnemySet enemySet;
        [SerializeField]
        private GameEvent onWaveEnded;
        [SerializeField]
        private float healthMultiplier = 0.5f;
        [SerializeField]
        private ExposedProperty onSucking;
        [SerializeField]
        private ExposedProperty onFinishSucking;

        private float currentHealthMultiplier = 1.0f;
        private int currentWaveConfig = 0;

        private WaveConfig CurrentWave => waveConfigs[currentWaveConfig];

#if UNITY_EDITOR
        internal List<EnemyPath> Paths => paths;
#endif

        /// <summary>
        /// Start Spawning coroutine
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
