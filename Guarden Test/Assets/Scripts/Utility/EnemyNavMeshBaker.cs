using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// EnemyNavMeshBaker bakes the enemy surface navmesh when force fields are added/removed to/from the scene
    /// </summary>
    [RequireComponent(typeof(NavMeshSurface))]
    public class EnemyNavMeshBaker : MonoBehaviour
    {
        [SerializeField, Tooltip("Autofilled. The Enemy NavmeshSurface")]
        private NavMeshSurface sceneSurface;

        private static NavMeshSurface surface;

        private void Awake()
        {
            surface = sceneSurface;
            BakeNavMesh();
        }

        /// <summary>
        /// Bakes the navmesh again to take into account current changes
        /// </summary>
        public static void BakeNavMesh()
        {
            //Run coroutine on scene object
            surface.StartCoroutine(BuildNavMesh());
        }

        /// <summary>
        /// Updates the enemy navmesh scene asynchronously
        /// </summary>
        /// <returns></returns>
        private static IEnumerator BuildNavMesh()
        {
            //Wait one frame for scene to update
            yield return null;

            GameLogger.LogInfo("Building enemy surface nav mesh", null, GameLogger.LogCategory.Scene);

            if (surface.navMeshData == null)
            {
                GameLogger.LogError("Enemy surface has no data", null, GameLogger.LogCategory.Scene);
                surface.BuildNavMesh();
                yield break;
            }

            surface.UpdateNavMesh(surface.navMeshData);
        }

#if UNITY_EDITOR
        internal void AutofillSurface()
        {
            sceneSurface = GetComponent<NavMeshSurface>();
        }
#endif
    }
}
