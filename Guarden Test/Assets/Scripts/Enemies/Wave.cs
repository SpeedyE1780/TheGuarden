using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Enemies
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemies/Wave")]
    public class Wave : ScriptableObject
    {
        [SerializeField, Tooltip("List of spawn groups that will be spawned in wave")]
        private List<SpawnGroup> spawnGroups = new List<SpawnGroup>();
        [SerializeField, Tooltip("Delay between each spawn group")]
        private float delay = 2.0f;

        internal IEnumerator SpawnWave(SpawnConfiguration configuration)
        {
            foreach (SpawnGroup spawnGroup in spawnGroups)
            {
                yield return spawnGroup.Spawn(configuration);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
