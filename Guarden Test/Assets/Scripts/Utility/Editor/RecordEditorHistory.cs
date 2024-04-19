using UnityEditor;
using UnityEngine;

namespace TheGuarden.Utility.Editor
{
    public static class RecordEditorHistory
    {
        public delegate void ModifyComponent();

        public static void RecordHistory<T>(T component, string undoTitle, ModifyComponent modifyComponent) where T : Component
        {
            Undo.RecordObject(component, undoTitle);
            modifyComponent();

            if(PrefabUtility.IsPartOfAnyPrefab(component))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(component);
            }
        }
    }
}
