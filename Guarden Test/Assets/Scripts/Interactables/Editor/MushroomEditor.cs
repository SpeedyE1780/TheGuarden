using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Interactable.Editor
{
    internal class MushroomEditor
    {
        [MenuItem("CONTEXT/Mushroom/Autofill variables")]
        internal static void AutofillVariables(MenuCommand command)
        {
            Mushroom mushroom = command.context as Mushroom;
            RecordEditorHistory.RecordHistory(mushroom, $"Autofill {mushroom.name} variables", mushroom.AutofillVariables);
        }
    }
}
