using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheGuarden.Utility;

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

        private bool ShouldSpawn => gameTime.Hour >= startTime && gameTime.Hour <= endTime;

        private void Start()
        {
            StartCoroutine(SpawnEnemy());
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
                    Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    enemy.SetPath(paths[Random.Range(0, paths.Count)]);
                    GameLogger.LogInfo("Enemy Spawned", this, GameLogger.LogCategory.Enemy);
                    yield return new WaitForSeconds(spawningDelay);
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
