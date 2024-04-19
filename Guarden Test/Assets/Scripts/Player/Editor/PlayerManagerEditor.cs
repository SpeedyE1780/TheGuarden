using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Players.Editor
{
    internal class PlayerManagerEditor
    {
        [MenuItem("CONTEXT/PlayerManager/Autofill Input Module")]
        internal static void AutofillInputModule(MenuCommand command)
        {
            PlayerManager manager = command.context as PlayerManager;
            RecordEditorHistory.RecordHistory(manager, $"Autofill {manager.name} input module", manager.FillInputModule);
        }
    }
}
