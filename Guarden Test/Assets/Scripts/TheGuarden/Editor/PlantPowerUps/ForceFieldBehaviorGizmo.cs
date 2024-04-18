using UnityEditor;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Editor
{
    public class ForceFieldBehaviorGizmo
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(ForceFieldBehavior forceFieldBehavior, GizmoType type)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(forceFieldBehavior.transform.position, forceFieldBehavior.PowerUpRange);
        }
    }
}
