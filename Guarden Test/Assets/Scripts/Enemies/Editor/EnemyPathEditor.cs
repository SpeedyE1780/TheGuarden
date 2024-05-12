using System;
using UnityEditor;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    public class EnemyPathEditor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(EnemyPath enemyPath, GizmoType gizmoType)
        {
            foreach (Transform point in enemyPath.Points)
            {
                Gizmos.DrawWireSphere(point.position, 1.0f);
            }
        }

        private static Transform CreateColliderTransform(Transform parent)
        {
            GameObject colliderParent = new GameObject("Colliders");
            colliderParent.transform.SetParent(parent);
            return colliderParent.transform;
        }

        private static Transform GetColliderParents(Transform enemyPathTransform)
        {
            Transform colliderParents = enemyPathTransform.Find("Colliders");
            return colliderParents ?? CreateColliderTransform(enemyPathTransform);
        }

        private static void DeleteChildren(Transform parent)
        {
            while (parent.childCount > 0)
            {
                Transform child = parent.GetChild(0);
                child.SetParent(null);
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }

        private static Tuple<Vector3, Quaternion> GetPositionAndRotation(Vector3 start, Vector3 end)
        {
            Vector3 middle = (start + end) * 0.5f;
            Quaternion rotation = Quaternion.LookRotation(end - start, Vector3.up);
            return new Tuple<Vector3, Quaternion>(middle, rotation);
        }

        private static BoxCollider CreateCollider(Transform parent, Tuple<Vector3, Quaternion> positionRotation, int layer, string name)
        {
            GameObject collider = new GameObject(name);
            collider.layer = layer;
            collider.transform.SetParent(parent);
            collider.transform.SetPositionAndRotation(positionRotation.Item1, positionRotation.Item2);
            return collider.AddComponent<BoxCollider>();
        }

        [MenuItem("CONTEXT/EnemyPath/Generate Colliders")]
        internal static void AutofillVariables(MenuCommand command)
        {
            EnemyPath enemyPath = command.context as EnemyPath;
            int layer = LayerMask.NameToLayer("EnemyPath");
            Transform colliderParent = GetColliderParents(enemyPath.transform);
            colliderParent.gameObject.layer = layer;
            DeleteChildren(colliderParent);

            for (int i = 0; i < enemyPath.Points.Count - 1; i++)
            {
                Vector3 current = enemyPath.Points[i].position;
                Vector3 next = enemyPath.Points[i + 1].position;
                float distance = (next - current).magnitude;

                BoxCollider collider = CreateCollider(colliderParent, GetPositionAndRotation(current, next), layer, $"Collider {i}");
                collider.size = new Vector3(enemyPath.pathWidth, 0.5f, distance);
                collider.isTrigger = true;
            }
        }
    }
}
