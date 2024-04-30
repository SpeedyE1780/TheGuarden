using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.NPC
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("Shed where animals should go to at night")]
        private Transform shed;
        [SerializeField, Tooltip("Animal prefab")]
        private Animal animalPrefab;
        [SerializeField, Tooltip("Animal spawned each day")]
        private int spawnCount;
        [SerializeField, Tooltip("Animal set")]
        private AnimalSet animalSet;

        private void OnEnable()
        {
            DayLightCycle.OnDayStarted += SpawnAnimals;
            Animal.Shed = shed;
        }

        private void OnDisable()
        {
            DayLightCycle.OnDayStarted -= SpawnAnimals;
        }

        private void SpawnAnimals()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Instantiate(animalPrefab, shed.position, Quaternion.identity);
                GameLogger.LogInfo("Animal Spawned", this, GameLogger.LogCategory.Scene);
            }
        }

        public void RemoveAnimal()
        {
            if (animalSet.Count > 0)
            {
                Destroy(animalSet[0].gameObject);
            }

            if (animalSet.Count == 0)
            {
                GameLogger.LogInfo("All animals taken", this, GameLogger.LogCategory.Scene);
            }
        }
    }
}
