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
        [SerializeField, Tooltip("Hour where enemies start spawning")]
        private float startTime;
        [SerializeField, Tooltip("Hour where enemies stop spawning")]
        private float endTime;
        [SerializeField, Tooltip("Delay between each enemy spawning")]
        private float spawningDelay;
        [SerializeField, Tooltip("Enemy Prefab")]
        private Enemy enemyPrefab;
        [SerializeField, Tooltip("Autofilled")]
        private GameTime gameTime;
        [SerializeField, Tooltip("UFO transform that moves it in the scene")]
        private Transform ufoTransform;
        [SerializeField, Tooltip("UFO Visual effect that will drop enemy")]
        private VisualEffect ufo;
        [SerializeField, Tooltip("UFO movement speed")]
        private float ufoSpeed = 75.0f;
        [SerializeField, Tooltip("Number of enemies spawned per ufo trip")]
        private int enemyCount = 3;

        private bool ShouldSpawn => gameTime.Hour >= startTime && gameTime.Hour <= endTime;

        private void Start()
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
            while (true)
            {
                yield return new WaitUntil(() => ShouldSpawn);

                while (ShouldSpawn)
                {
                    Vector3 startPosition = ufoTransform.position;
                    Vector3 endPosition = spawnPoint.position - startPosition;
                    ufoTransform.gameObject.SetActive(true);
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

                    yield return MoveUFO(endPosition);

                    ufo.Stop();
                    ufoTransform.gameObject.SetActive(false);
                }
            }
        }

#if UNITY_EDITOR
        internal void AutofillGameTime()
        {
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("Game Time not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
