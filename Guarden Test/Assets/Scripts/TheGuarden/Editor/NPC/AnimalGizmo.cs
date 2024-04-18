using UnityEditor;
using UnityEngine;

namespace TheGuarden.NPC.Editor
{
    public class AnimalGizmo
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        public static void DrawGizmo(Animal animal, GizmoType type)
        {
            Gizmos.color = animal.InsideForceField ? Color.green : Color.red;
            Gizmos.DrawWireSphere(animal.transform.position, 1.5f);
        }
    }
}
