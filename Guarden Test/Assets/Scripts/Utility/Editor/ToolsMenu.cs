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

            NavMeshSurface target = selectedObject.GetComponent<NavMeshSurface>() ?? selectedObject.AddComponent<NavMeshSurface>();
            target.center = source.center;
            target.size = source.size;
        }

        private static void ApplyBoxPropertiesFromNavMeshSurface(GameObject selectedObject)
        {
            NavMeshSurface source = selectedObject.GetComponent<NavMeshSurface>();

            if (source == null)
            {
                GameLogger.LogError($"{selectedObject.name} has no NavMeshSurface", selectedObject, GameLogger.LogCategory.Editor);
                return;
            }

            BoxCollider target = selectedObject.GetComponent<BoxCollider>() ?? selectedObject.AddComponent<BoxCollider>();
            target.center = source.center;
            target.size = source.size;
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
