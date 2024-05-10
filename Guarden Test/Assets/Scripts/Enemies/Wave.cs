using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// Wave is a list of SpawnGroup that needs to be spawned in the same night
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemies/Wave")]
    internal class Wave : ScriptableObject
    {
        [SerializeField, Tooltip("List of spawn groups that will be spawned in wave")]
        private List<SpawnGroup> spawnGroups = new List<SpawnGroup>();
        [SerializeField, Tooltip("Delay between each spawn group")]
        private float delay = 2.0f;

        /// <summary>
        /// Spawn every group in wave
        /// </summary>
        /// <param name="configuration">Config object used to intialize spawned enemy</param>
        /// <returns></returns>
        internal IEnumerator SpawnWave(SpawnConfiguration configuration)
        {
            GameLogger.LogInfo($"Wave {name} spawning", this, GameLogger.LogCategory.Enemy);

            for (int i = 0; i < spawnGroups.Count; i++)
            {
                SpawnGroup spawnGroup = spawnGroups[i];
                yield return spawnGroup.Spawn(configuration);

                if (i + 1 < spawnGroups.Count)
                {
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}
