using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TheGuarden.Interactable.Editor
{
    internal class GrowPlantEditor : MonoBehaviour
    {
        [MenuItem("CONTEXT/GrowPlant/Autofill Variables")]
        internal static void AutofillVariables(MenuCommand command)
        {
            GrowPlant growPlant = command.context as GrowPlant;
            growPlant.AutofillVariables();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("CONTEXT/GrowPlant/Validate Variables")]
        internal static void ValidateVariables(MenuCommand command)
        {
            GrowPlant growPlant = command.context as GrowPlant;
            growPlant.ValidateVariables();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
