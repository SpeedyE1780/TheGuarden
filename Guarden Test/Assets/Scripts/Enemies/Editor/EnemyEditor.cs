using UnityEditor;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    internal class EnemyEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(Enemy enemy, GizmoType type)
        {
            if(type == GizmoType.InSelectionHierarchy)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(enemy.transform.position, enemy.TargetPosition);
            }

            Gizmos.DrawWireSphere(enemy.transform.position, enemy.DetectionRadius);
        }
    }
}
