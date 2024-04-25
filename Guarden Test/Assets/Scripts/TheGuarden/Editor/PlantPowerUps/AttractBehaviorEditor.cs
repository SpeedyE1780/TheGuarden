using UnityEngine;
using UnityEditor;

namespace TheGuarden.PlantPowerUps.Editor
{
    public class AttractBehaviorEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(AttractBehavior attractBehavior, GizmoType type)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attractBehavior.transform.position, attractBehavior.PowerUpRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attractBehavior.transform.position, attractBehavior.MaximumRange);
            Gizmos.DrawWireSphere(attractBehavior.transform.position, attractBehavior.MinimumRange);
        }
    }
}
