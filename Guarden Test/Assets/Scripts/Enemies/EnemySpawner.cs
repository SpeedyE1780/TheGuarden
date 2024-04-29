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
        [SerializeField, Tooltip("Delay between each enemy spawning")]
        private float spawningDelay;
        [SerializeField, Tooltip("Enemy Prefab")]
        private Enemy enemyPrefab;
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

        private List<GameObject> spawnedEnemies = new List<GameObject>();

        public UnityEvent OnWaveCompleted;

#if UNITY_EDITOR
        internal List<EnemyPath> Paths => paths;
#endif

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

        private void SpawnEnemy()
        {
            Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.SetPath(paths.GetRandomItem());
            enemy.OnPathEnded = (enemyObject) => spawnedEnemies.Remove(enemyObject);
            spawnedEnemies.Add(enemy.gameObject);
            GameLogger.LogInfo("Enemy Spawned", this, GameLogger.LogCategory.Enemy);
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

            for (int i = 0; i < enemyCount; i++)
            {
                yield return new WaitForSeconds(spawningDelay);
                SpawnEnemy();
            }

            ufo.SendEvent("OnFinishSucking");
            followCamera.RemoveTarget(ufoTransform);
            yield return MoveUFO(endPosition);

            ufo.Stop();
            ufoTransform.gameObject.SetActive(false);

            yield return new WaitUntil(() => spawnedEnemies.Count == 0);
            OnWaveCompleted.Invoke();
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
