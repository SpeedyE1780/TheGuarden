using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPoint;
        [SerializeField]
        private List<EnemyPath> paths;
        [SerializeField]
        private float startTime;
        [SerializeField]
        private float endTime;
        [SerializeField]
        private float spawningDelay;
        [SerializeField]
        private Enemy enemyPrefab;
        [SerializeField]
        private GameTime gameTime;

        private void Start()
        {
            StartCoroutine(SpawnEnemy());
        }

        private bool ShouldSpawn()
        {
            return gameTime.Hour >= startTime && gameTime.Hour <= endTime;
        }

        private IEnumerator SpawnEnemy()
        {
            while (true)
            {
                yield return new WaitUntil(ShouldSpawn);

                while (ShouldSpawn())
                {
                    Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    enemy.Path = paths[Random.Range(0, paths.Count)];
                    GameLogger.LogInfo("Enemy Spawned", this, GameLogger.LogCategory.Enemy);
                    yield return new WaitForSeconds(spawningDelay);
                }
            }
        }

        private void OnValidate()
        {
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("Game Time not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
    } 
}
