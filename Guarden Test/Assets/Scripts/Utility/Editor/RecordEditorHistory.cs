using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TheGuarden.Utility.Editor
{
    public static class RecordEditorHistory
    {
        public delegate void ModifyComponent();

        public static void MarkSceneDirty()
        {
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        public static void RecordHistory<T>(T component, string undoTitle, ModifyComponent modifyComponent) where T : Object
        {
            Undo.RecordObject(component, undoTitle);
            modifyComponent();

            if (PrefabUtility.IsPartOfAnyPrefab(component))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(component);
            }
        }
    }
}
