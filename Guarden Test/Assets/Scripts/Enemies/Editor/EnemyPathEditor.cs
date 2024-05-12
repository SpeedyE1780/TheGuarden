using System;
using UnityEditor;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    public class EnemyPathEditor
    {
        private const string ColliderParent = "Colliders";
        private const string ColliderName = "Collider";
        private const string ForceFieldParent = "ForceFields";
        private const string ForceFieldName = "ForceField";


        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        internal static void DrawGizmo(EnemyPath enemyPath, GizmoType gizmoType)
        {
            foreach (Transform point in enemyPath.Points)
            {
                Gizmos.DrawWireSphere(point.position, 1.0f);
            }
        }

        private static Transform CreateParent(Transform parent, string name)
        {
            GameObject colliderParent = new GameObject(name);
            colliderParent.transform.SetParent(parent);
            return colliderParent.transform;
        }

        private static Transform GetParent(Transform enemyPathTransform, string name)
        {
            Transform colliderParents = enemyPathTransform.Find(name);
            return colliderParents ?? CreateParent(enemyPathTransform, name);
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
        internal static void GenerateColliders(MenuCommand command)
        {
            EnemyPath enemyPath = command.context as EnemyPath;
            int layer = LayerMask.NameToLayer("EnemyPath");
            Transform colliderParent = GetParent(enemyPath.transform, ColliderParent);
            colliderParent.gameObject.layer = layer;
            DeleteChildren(colliderParent);

            for (int i = 0; i < enemyPath.Points.Count - 1; i++)
            {
                Vector3 current = enemyPath.Points[i].position;
                Vector3 next = enemyPath.Points[i + 1].position;
                float distance = (next - current).magnitude;

                BoxCollider collider = CreateCollider(colliderParent, GetPositionAndRotation(current, next), layer, $"{ColliderName} {i}");
                collider.size = new Vector3(enemyPath.pathWidth, 0.5f, distance);
                collider.isTrigger = true;
            }
        }

        [MenuItem("CONTEXT/EnemyPath/Generate Force Fields")]
        internal static void GenerateForceFields(MenuCommand command)
        {
            EnemyPath enemyPath = command.context as EnemyPath;
            Transform colliderParent = GetParent(enemyPath.transform, ColliderParent);

            if (colliderParent.childCount == 0)
            {
                GenerateColliders(command);
            }

            Transform forceFieldParent = GetParent(enemyPath.transform, ForceFieldParent);
            DeleteChildren(forceFieldParent);

            for (int i = 0; i < colliderParent.childCount; i++)
            {
                GameObject forceField = UnityEngine.Object.Instantiate(enemyPath.forceFieldPrefab, forceFieldParent);
                BoxCollider collider = colliderParent.GetChild(i).GetComponent<BoxCollider>();
                forceField.name = $"{ForceFieldName} {i}";
                forceField.transform.SetPositionAndRotation(collider.transform.position, collider.transform.rotation);
                forceField.transform.localScale = collider.size;
            }
        }
    }
}
