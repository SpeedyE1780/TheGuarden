using UnityEditor;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Editor
{
    public class PlantTurretEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(PlantTurret plantTurret, GizmoType type)
        {
            Gizmos.DrawWireSphere(plantTurret.transform.position, plantTurret.PowerUpRange);

            if(plantTurret.TargetEnemy != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(plantTurret.transform.position, plantTurret.TargetEnemy.position);
                Gizmos.DrawWireSphere(plantTurret.TargetEnemy.position, 1.5f);
            }
        }
    }
}
