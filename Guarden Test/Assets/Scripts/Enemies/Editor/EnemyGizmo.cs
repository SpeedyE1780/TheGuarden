using UnityEditor;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    public class EnemyGizmo
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        public static void DrawGizmo(Enemy enemy, GizmoType type)
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
