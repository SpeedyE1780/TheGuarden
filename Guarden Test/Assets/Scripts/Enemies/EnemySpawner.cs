using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheGuarden.Utility;
using UnityEngine.VFX;
using UnityEngine.Events;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// EnemySpawner spawns enemy during time period
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
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
        private Wave wave;
        [SerializeField, Tooltip("List of all enemies in scene")]
        private EnemySet enemySet;

        public UnityEvent OnWaveCompleted;
        public UnityEvent OnEnemyReachedShed;

#if UNITY_EDITOR
        internal List<EnemyPath> Paths => paths;
#endif

        private void OnEnable()
        {
            DayLightCycle.OnNightStarted += StartSpawning;
            DayLightCycle.OnWaveEnded += OnWaveCompleted.Invoke;
        }

        private void OnDisable()
        {
            DayLightCycle.OnNightStarted -= StartSpawning;
            DayLightCycle.OnWaveEnded -= OnWaveCompleted.Invoke;
        }

        /// <summary>
        /// Start Spawning coroutine
        /// </summary>
        public void StartSpawning()
        {
            StartCoroutine(SpawnEnemies());
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
            ufo.SendEvent("OnSucking");

            SpawnConfiguration configuration = new SpawnConfiguration()
            {
                paths = paths,
                position = spawnPoint.position,
                rotation = spawnPoint.rotation,
                OnDestroyed = (enemyObject) =>
                {
                    OnEnemyReachedShed.Invoke();
                }
            };

            yield return wave.SpawnWave(configuration);

            ufo.SendEvent("OnFinishSucking");
            followCamera.RemoveTarget(ufoTransform);
            yield return MoveUFO(endPosition);

            ufo.Stop();
            ufoTransform.gameObject.SetActive(false);

            yield return new WaitUntil(() => enemySet.Count == 0);
            DayLightCycle.EnemyWaveEnded();
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
