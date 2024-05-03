using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Enemies
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemies/Spawn Group")]
    internal class SpawnGroup : ScriptableObject
    {
        [SerializeField, Tooltip("List of enemies that will be spawned together")]
        private List<ObjectPool<Enemy>> enemies = new List<ObjectPool<Enemy>>();
        [SerializeField, Tooltip("Delay between each enemy spawning")]
        private float delay = 0.5f;

        internal IEnumerator Spawn(SpawnConfiguration configuration)
        {
            foreach (ObjectPool<Enemy> enemyPool in enemies)
            {
                Enemy enemy = enemyPool.GetPooledObject();
                enemy.transform.SetPositionAndRotation(configuration.position, configuration.rotation);
                enemy.SetPath(configuration.paths.GetRandomItem());
                enemy.Health.MutlitplyMaxHealth(configuration.healthMultiplier);
                GameLogger.LogInfo("Enemy Spawned", this, GameLogger.LogCategory.Enemy);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
