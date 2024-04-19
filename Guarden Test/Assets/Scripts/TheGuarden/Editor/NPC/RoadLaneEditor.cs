using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TheGuarden.NPC.Editor
{
    internal class RoadLaneEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(RoadLane roadlane, GizmoType type)
        {
            if (roadlane.Start != null && roadlane.End != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(roadlane.Start.position, 0.5f);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(roadlane.End.position, 0.5f);
            }
        }

        [MenuItem("CONTEXT/RoadLane/Autofill Variables")]
        internal static void AutofillVariables(MenuCommand command)
        {
            RoadLane road = command.context as RoadLane;
            road.AutofillVariables();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
