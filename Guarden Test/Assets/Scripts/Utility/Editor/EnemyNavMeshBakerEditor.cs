using UnityEditor;

namespace TheGuarden.Utility.Editor
{
    internal class EnemyNavMeshBakerEditor
    {
        [MenuItem("CONTEXT/EnemyNavMeshBaker/Autofill NavMeshSurface")]
        internal static void AutofillNavMeshSurface(MenuCommand command)
        {
            EnemyNavMeshBaker enemyNavMeshBaker = command.context as EnemyNavMeshBaker;
            RecordEditorHistory.RecordHistory(enemyNavMeshBaker, $"Set {enemyNavMeshBaker.name} NavMeshSurface", enemyNavMeshBaker.AutofillSurface);
        }
    }
}
