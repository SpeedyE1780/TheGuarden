using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// SpawnGroup is a list of enemies that will be spawned together
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemies/Spawn Group")]
    internal class SpawnGroup : ScriptableObject
    {
        [SerializeField, Tooltip("List of enemies that will be spawned together")]
        private List<ObjectPool<Enemy>> enemies = new List<ObjectPool<Enemy>>();
        [SerializeField, Tooltip("Delay between each enemy spawning")]
        private float delay = 0.5f;

        /// <summary>
        /// Retrieve enemy from pool and set basic info
        /// </summary>
        /// <param name="pool">Pool from which enemy is retrieved</param>
        /// <param name="configuration">Basic enemy info</param>
        private void SpawnEnemy(ObjectPool<Enemy> pool, SpawnConfiguration configuration)
        {
            Enemy enemy = pool.GetPooledObject();
            enemy.transform.SetPositionAndRotation(configuration.position, configuration.rotation);
            enemy.Agent.Warp(configuration.position);
            enemy.SetPath(configuration.paths.GetRandomItem());
            enemy.Health.MutlitplyMaxHealth(configuration.healthMultiplier);
            GameLogger.LogInfo("Enemy Spawned", this, GameLogger.LogCategory.Enemy);
        }

        /// <summary>
        /// Spawn all enemies in group from pool
        /// </summary>
        /// <param name="configuration">Config object used to intialize spawned enemy</param>
        /// <returns></returns>
        internal IEnumerator Spawn(SpawnConfiguration configuration)
        {
            GameLogger.LogInfo($"Spawning {name} group", this, GameLogger.LogCategory.Enemy);

            for (int i = 0; i < enemies.Count; i++)
            {
                SpawnEnemy(enemies[i], configuration);

                if (i + 1 < enemies.Count)
                {
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}
