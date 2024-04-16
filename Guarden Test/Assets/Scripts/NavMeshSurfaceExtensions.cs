using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshSurfaceExtensions : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface sceneSurface;

    private static NavMeshSurface surface;

    private void Awake()
    {
        surface = sceneSurface;
    }

    public static Vector3 GetPointOnSurface()
    {
        Bounds surfaceBounds = surface.navMeshData.sourceBounds;
        float x = Random.Range(surfaceBounds.center.x + surfaceBounds.min.x, surfaceBounds.center.x + surfaceBounds.max.x);
        float z = Random.Range(surfaceBounds.center.z + surfaceBounds.min.z, surfaceBounds.center.z + surfaceBounds.max.z);

        return new Vector3(x, 0, z);
    }

    private void OnValidate()
    {
        sceneSurface = GetComponent<NavMeshSurface>();
    }
}
