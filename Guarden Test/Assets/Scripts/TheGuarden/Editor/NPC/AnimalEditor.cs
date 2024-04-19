using UnityEditor;
using UnityEngine;

namespace TheGuarden.NPC.Editor
{
    internal class AnimalEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(Animal animal, GizmoType type)
        {
            Gizmos.color = animal.InsideForceField ? Color.green : Color.red;
            Gizmos.DrawWireSphere(animal.transform.position, 1.5f);
        }

        [MenuItem("CONTEXT/Animal/Autofill components")]
        internal static void AutofillComponents(MenuCommand command)
        {
            Animal animal = command.context as Animal;
            animal.AutofillComponents();
        }
    }
}
