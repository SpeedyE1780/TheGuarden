using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheGuarden.Utility.Editor
{
    internal class EnemyNavMeshBakerEditor
    {
        [MenuItem("CONTEXT/EnemyNavMeshBaker/Autofill NavMeshSurface")]
        internal static void AutofillNavMeshSurface(MenuCommand command)
        {
            EnemyNavMeshBaker enemyNavMeshBaker = command.context as EnemyNavMeshBaker;
            enemyNavMeshBaker.AutofillSurface();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
