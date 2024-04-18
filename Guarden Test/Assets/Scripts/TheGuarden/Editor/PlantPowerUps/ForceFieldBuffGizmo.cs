using UnityEditor;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Editor
{
    public class ForceFieldBuffGizmo
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(ForceFieldBuff forceFieldBuff, GizmoType type)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(forceFieldBuff.transform.position, forceFieldBuff.PowerUpRange);
        }
    }
}
