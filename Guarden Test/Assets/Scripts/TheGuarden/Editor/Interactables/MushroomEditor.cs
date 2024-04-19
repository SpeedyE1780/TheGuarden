using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TheGuarden.Interactable.Editor
{
    internal class MushroomEditor
    {
        [MenuItem("CONTEXT/Mushroom/Autofill variables")]
        internal static void AutofillVariables(MenuCommand command)
        {
            Mushroom mushroom = command.context as Mushroom;
            mushroom.AutofillVariables();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
