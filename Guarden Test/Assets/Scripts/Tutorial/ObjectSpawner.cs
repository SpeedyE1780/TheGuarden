using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/Spawn Object")]
    internal class ObjectSpawner : ScriptableObject
    {
        [SerializeField, Tooltip("Object that needs to be spawned in the scene")]
        private GameObject objectPrefab;

        private GameObject spawnedObject;

        internal GameObject SpawnedObject
        {
            get
            {
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(objectPrefab);
                    GameLogger.LogInfo($"{name}: spawning {objectPrefab.name} in scene", this, GameLogger.LogCategory.Tutorial);
                }

                return spawnedObject;
            }
        }
    }
}
