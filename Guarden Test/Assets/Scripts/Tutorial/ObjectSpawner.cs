using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// ObjectSpawner spawns an object in the scene and allows other tutorials to access it
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Object Spawner")]
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
                    spawnedObject.transform.position = new Vector3 (0, 0, -6);
                    GameLogger.LogInfo($"{name}: spawning {objectPrefab.name} in scene", this, GameLogger.LogCategory.Tutorial);
                }

                return spawnedObject;
            }
        }
    }
}
