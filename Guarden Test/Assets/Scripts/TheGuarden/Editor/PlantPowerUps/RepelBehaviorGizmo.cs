using UnityEditor;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Editor
{
    public class RepelBehaviorGizmo
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(RepelBehavior repelBehavior, GizmoType type)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(repelBehavior.transform.position, repelBehavior.PowerUpRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(repelBehavior.transform.position, repelBehavior.RepelRange);
            Gizmos.DrawWireSphere(repelBehavior.transform.position, repelBehavior.MinimumRange + repelBehavior.PowerUpRange);
        }
    }
}
