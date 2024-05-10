using TheGuarden.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace TheGuarden.NPC.Editor
{
    internal class AnimalEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy)]
        internal static void DrawGizmo(Animal animal, GizmoType type)
        {
            Gizmos.DrawLine(animal.transform.position, animal.Agent.destination);
        }

        [MenuItem("CONTEXT/Animal/Autofill variables")]
        internal static void AutofillComponents(MenuCommand command)
        {
            Animal animal = command.context as Animal;
            RecordEditorHistory.RecordHistory(animal, $"Autofill {animal.name} variables", animal.AutofillComponents);
        }
    }
}
