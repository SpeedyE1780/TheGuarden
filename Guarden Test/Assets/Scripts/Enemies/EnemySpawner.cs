using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheGuarden.Utility;
using UnityEngine.VFX;

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
        [SerializeField, Tooltip("Hours where enemies will spawn")]
        private List<int> spawnHours;
        [SerializeField, Tooltip("Delay between each enemy spawning")]
        private float spawningDelay;
        [SerializeField, Tooltip("Enemy Prefab")]
        private Enemy enemyPrefab;
        [SerializeField, Tooltip("Autofilled")]
        private GameTime gameTime;
        [SerializeField, Tooltip("Autofilled. Camera following players")]
        private FollowTarget followCamera;
        [SerializeField, Tooltip("UFO transform that moves it in the scene")]
        private Transform ufoTransform;
        [SerializeField, Tooltip("UFO Visual effect that will drop enemy")]
        private VisualEffect ufo;
        [SerializeField, Tooltip("UFO movement speed")]
        private float ufoSpeed = 75.0f;
        [SerializeField, Tooltip("Number of enemies spawned per ufo trip")]
        private int enemyCount = 3;

#if UNITY_EDITOR
        internal List<EnemyPath> Paths => paths;
#endif

        private void Start()
        {
            StartCoroutine(SpawnEnemy());
        }

        private void OnEnable()
        {
            gameTime.OnDayEnded += QueueSpawn;
        }

        private void OnDisable()
        {
            gameTime.OnDayEnded -= QueueSpawn;
        }

        /// <summary>
        /// Start Spawning coroutine
        /// </summary>
        private void QueueSpawn()
        {
            StartCoroutine(SpawnEnemy());
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
        /// Wait for spawning period to start and spawn enemies
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnEnemy()
        {
            foreach (int spawnHour in spawnHours)
            {
                yield return new WaitUntil(() => gameTime.Hour >= spawnHour);

                Vector3 startPosition = ufoTransform.position;
                Vector3 endPosition = spawnPoint.position - startPosition;
                ufoTransform.gameObject.SetActive(true);
                followCamera.AddTarget(ufoTransform);
                ufo.Stop();

                yield return MoveUFO(spawnPoint.position);

                ufo.Play();

                for (int i = 0; i < enemyCount; i++)
                {
                    yield return new WaitForSeconds(spawningDelay);
                    Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    enemy.SetPath(paths[Random.Range(0, paths.Count)]);
                    GameLogger.LogInfo("Enemy Spawned", this, GameLogger.LogCategory.Enemy);
                }

                followCamera.RemoveTarget(ufoTransform);
                yield return MoveUFO(endPosition);

                ufo.Stop();
                ufoTransform.gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        internal void AutofillVariables()
        {
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("Game Time not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }

            followCamera = FindObjectOfType<FollowTarget>();

            if (followCamera == null)
            {
                GameLogger.LogWarning("Follow Camera not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
