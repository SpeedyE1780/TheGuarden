using UnityEditor;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Editor
{
    public class PlantBehaviorEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(PlantBehavior plantBehavior, GizmoType type)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(plantBehavior.transform.position, plantBehavior.PowerUpRange);
        }
    }
}
