using UnityEditor;
using UnityEngine;
using Unity.AI.Navigation;

namespace TheGuarden.Utility.Editor
{
    internal class ToolsMenu : MonoBehaviour
    {
        private static void ApplyBoxPropertiesFromBoxCollider(GameObject selectedObject)
        {
            BoxCollider source = selectedObject.GetComponent<BoxCollider>();

            if (source == null)
            {
                GameLogger.LogError($"{selectedObject.name} has no BoxCollider", selectedObject, GameLogger.LogCategory.Editor);
                return;
            }

            NavMeshSurface target = selectedObject.GetComponent<NavMeshSurface>();

            if (target == null)
            {
                RecordEditorHistory.RecordHistory(selectedObject.transform, $"Add NavMeshSurface to {selectedObject.name}", () => target = selectedObject.AddComponent<NavMeshSurface>());
            }

            RecordEditorHistory.RecordHistory(target, "Update NavMeshSurface dimension", () =>
            {
                target.center = source.center;
                target.size = source.size;
            });
        }

        private static void ApplyBoxPropertiesFromNavMeshSurface(GameObject selectedObject)
        {
            NavMeshSurface source = selectedObject.GetComponent<NavMeshSurface>();

            if (source == null)
            {
                GameLogger.LogError($"{selectedObject.name} has no NavMeshSurface", selectedObject, GameLogger.LogCategory.Editor);
                return;
            }

            BoxCollider target = selectedObject.GetComponent<BoxCollider>();

            if (target == null)
            {
                RecordEditorHistory.RecordHistory(selectedObject.transform, $"Add BoxCollider to {selectedObject.name}", () => target = selectedObject.AddComponent<BoxCollider>());
            }

            RecordEditorHistory.RecordHistory(target, "Update BoxCollider dimension", () =>
            {
                target.center = source.center;
                target.size = source.size;
            });
        }

        [MenuItem("The Guarden/Tools/Add Box Collider from Nav Mesh Surface")]
        internal static void AddBoxCollider()
        {
            foreach (GameObject selectedObject in Selection.gameObjects)
            {
                ApplyBoxPropertiesFromNavMeshSurface(selectedObject);
            }
        }

        [MenuItem("The Guarden/Tools/Add Nav Mesh Surface from Box Collider")]
        internal static void AddNavMeshSurface()
        {
            foreach (GameObject selectedObject in Selection.gameObjects)
            {
                ApplyBoxPropertiesFromBoxCollider(selectedObject);
            }
        }
    }
}
