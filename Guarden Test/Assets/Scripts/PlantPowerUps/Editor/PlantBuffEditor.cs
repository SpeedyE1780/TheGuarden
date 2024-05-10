using UnityEditor;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Buffs.Editor
{
    public class PlantBuffEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(PlantBuff forceFieldBuff, GizmoType type)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(forceFieldBuff.transform.position, forceFieldBuff.PowerUpRange);
        }
    }
}
