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
        private List<Enemy> enemies = new List<Enemy>();
        [SerializeField, Tooltip("Delay between each enemy spawning")]
        private float delay = 0.5f;

        internal IEnumerator Spawn(SpawnConfiguration configuration)
        {
            foreach (Enemy enemyPrefab in enemies)
            {
                Enemy enemy = Instantiate(enemyPrefab, configuration.position, configuration.rotation);
                enemy.SetPath(configuration.paths.GetRandomItem());
                enemy.OnReachShed = configuration.OnReachShed;
                GameLogger.LogInfo("Enemy Spawned", this, GameLogger.LogCategory.Enemy);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
