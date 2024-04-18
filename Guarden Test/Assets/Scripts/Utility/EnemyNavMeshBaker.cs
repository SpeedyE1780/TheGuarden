using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class EnemyNavMeshBaker : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface sceneSurface;

    private static NavMeshSurface surface;

    private void Awake()
    {
        surface = sceneSurface;
        BakeNavMesh();
    }

    public static void BakeNavMesh()
    {
        //Run coroutine on scene object
        surface.StartCoroutine(BuildNavMesh());
    }

    private static IEnumerator BuildNavMesh()
    {
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
}
