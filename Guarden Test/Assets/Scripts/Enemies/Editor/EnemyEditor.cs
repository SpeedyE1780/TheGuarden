using UnityEditor;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    internal class EnemyEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(Enemy enemy, GizmoType type)
        {
            Gizmos.DrawLine(enemy.transform.position, enemy.Agent.destination);

            if (enemy.Rewinding)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(enemy.transform.position, 2.0f);
            }
        }
    }
}
