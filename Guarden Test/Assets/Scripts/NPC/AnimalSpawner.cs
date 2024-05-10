using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.NPC
{
    internal class AnimalSpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("Shed where animals should go to at night")]
        private Transform shed;
        [SerializeField, Tooltip("Animal spawned each day")]
        private int spawnCount;
        [SerializeField, Tooltip("Animal spawned when game starts")]
        private int dayOneAnimalCount = 5;
        [SerializeField, Tooltip("Animal set")]
        private AnimalSet animalSet;
        [SerializeField, Tooltip("Game event raised when an animal is spawned/kidnapped")]
        private IntGameEvent onAnimalCountChanged;
        [SerializeField, Tooltip("Game event raised when all animals are kidnapped")]
        private GameEvent onGameEnded;
        [SerializeField, Tooltip("Pool from which animal are taken")]
        private ObjectPool<Animal> animalPool;

        private void Awake()
        {
            Animal.Shed = shed;
        }
        
        /// <summary>
        /// Called from OnGameStartedEvent
        /// </summary>
        public void SpawnDayOneAnimals()
        {
            SpawnAnimals(dayOneAnimalCount);
        }

        /// <summary>
        /// Called from OnDayStartedEvent
        /// </summary>
        public void SpawnAnimals()
        {
            SpawnAnimals(spawnCount);
        }

        /// <summary>
        /// Spawn animals from pool
        /// </summary>
        /// <param name="count">Number of animals to spawn</param>
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

        /// <summary>
        /// Called from OnEnemyReachShed event
        /// </summary>
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
