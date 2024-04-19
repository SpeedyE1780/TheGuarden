using Unity.AI.Navigation;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Extension class that will return random point within navmesh surface bounds
    /// </summary>
    [RequireComponent(typeof(NavMeshSurface))]
    public class NavMeshSurfaceExtensions : MonoBehaviour
    {
        [SerializeField, Tooltip("Autofilled. The surface who's bounds are used")]
        private NavMeshSurface sceneSurface;

        private static NavMeshSurface surface;

        private void Awake()
        {
            surface = sceneSurface;
        }

        /// <summary>
        /// Get a random point on the navmesh surface
        /// </summary>
        /// <returns>A random point on the navmesh surface</returns>
        public static Vector3 GetPointOnSurface()
        {
            Bounds surfaceBounds = surface.navMeshData.sourceBounds;
            float x = Random.Range(surfaceBounds.center.x + surfaceBounds.min.x, surfaceBounds.center.x + surfaceBounds.max.x);
            float z = Random.Range(surfaceBounds.center.z + surfaceBounds.min.z, surfaceBounds.center.z + surfaceBounds.max.z);

            return new Vector3(x, 0, z);
        }

#if UNITY_EDITOR
        internal void AutofillSurface()
        {
            sceneSurface = GetComponent<NavMeshSurface>();
        }
#endif
    }
}
