using UnityEditor;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    internal class EnemyEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy)]
        internal static void DrawGizmo(Enemy enemy, GizmoType type)
        {
            Gizmos.DrawLine(enemy.transform.position, enemy.Agent.destination);
        }
    }
}
