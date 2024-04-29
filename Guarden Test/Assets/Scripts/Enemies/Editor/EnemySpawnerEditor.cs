using TheGuarden.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    internal class EnemySpawnerEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(EnemySpawner enemySpawner, GizmoType gizmoType)
        {
            foreach (EnemyPath path in enemySpawner.Paths)
            {
                foreach (Transform point in path.Points)
                {
                    Gizmos.DrawWireSphere(point.position, 1.0f);
                }
            }
        }

        [MenuItem("CONTEXT/EnemySpawner/Autofill Variables")]
        internal static void AutofillVariables(MenuCommand command)
        {
            EnemySpawner spawner = command.context as EnemySpawner;
            RecordEditorHistory.RecordHistory(spawner, $"Autofill {spawner.name} Variables", spawner.AutofillVariables);
        }
    }
}
