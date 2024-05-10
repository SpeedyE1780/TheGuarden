using TheGuarden.Utility;
using TheGuarden.Utility.Events;
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
        [SerializeField]
        private int dayOneAnimalCount = 5;
        [SerializeField, Tooltip("Animal set")]
        private AnimalSet animalSet;
        [SerializeField]
        private IntGameEvent onAnimalCountChanged;
        [SerializeField]
        private GameEvent onGameEnded;
        [SerializeField]
        private ObjectPool<Animal> animalPool;

        private void Awake()
        {
            Animal.Shed = shed;
        }

        public void SpawnDayOneAnimals()
        {
            SpawnAnimals(dayOneAnimalCount);
        }

        public void SpawnAnimals()
        {
            SpawnAnimals(spawnCount);
        }

        private void SpawnAnimals(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Animal animal = animalPool.GetPooledObject();
                animal.transform.SetPositionAndRotation(shed.position, Quaternion.identity);
                GameLogger.LogInfo("Animal Spawned", this, GameLogger.LogCategory.Scene);
            }

            onAnimalCountChanged.Raise(animalSet.Count);
        }

        public void RemoveAnimal()
        {
            if (animalSet.Count > 0)
            {
                animalPool.AddObject(animalSet[0]);
            }

            onAnimalCountChanged.Raise(animalSet.Count);

            if (animalSet.Count == 0)
            {
                GameLogger.LogInfo("All animals taken", this, GameLogger.LogCategory.Scene);
                onGameEnded.Raise();
            }
        }
    }
}
