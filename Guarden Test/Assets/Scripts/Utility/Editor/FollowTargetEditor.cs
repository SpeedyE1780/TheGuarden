using UnityEditor;
using UnityEngine;

namespace TheGuarden.Utility.Editor
{
    public class FollowTargetEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(FollowTarget followTarget, GizmoType type)
        {
            if (type == GizmoType.InSelectionHierarchy)
            {
                Mesh cube = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
                Gizmos.DrawMesh(cube, followTarget.DefaultTargetPosition + Vector3.up * 0.5f);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(followTarget.Bounds.center, followTarget.Bounds.size);
        }

        [MenuItem("CONTEXT/FollowTarget/Calculate Offset")]
        internal static void CalculateOffset(MenuCommand command)
        {
            FollowTarget followTarget = command.context as FollowTarget;
            Vector3 offset = followTarget.transform.position - followTarget.DefaultTargetPosition;
            RecordEditorHistory.RecordHistory(followTarget, $"Update {followTarget.name} offsets", () => { followTarget.UpdateOffset(offset); });
        }
    }
}
