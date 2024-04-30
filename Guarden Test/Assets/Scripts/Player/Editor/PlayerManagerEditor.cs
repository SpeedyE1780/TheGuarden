using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Players.Editor
{
    internal class PlayerManagerEditor
    {
        [MenuItem("CONTEXT/PlayerManager/Autofill Components")]
        internal static void AutofillComponents(MenuCommand command)
        {
            PlayerManager manager = command.context as PlayerManager;
            RecordEditorHistory.RecordHistory(manager, $"Autofill {manager.name} components", manager.AutofillVariables);
        }
    }
}
