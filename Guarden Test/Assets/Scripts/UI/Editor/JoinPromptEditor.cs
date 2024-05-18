using UnityEditor;
using TheGuarden.Utility.Editor;

namespace TheGuarden.UI.Editor
{
    internal class JoinPromptEditor
    {
        [MenuItem("CONTEXT/JoinPrompt/Set Prompt")]
        internal static void AutofillText(MenuCommand command)
        {
            JoinPrompt joinPrompt = command.context as JoinPrompt;
            RecordEditorHistory.RecordHistory(joinPrompt, $"Set {joinPrompt.name} instruction text", joinPrompt.SetText);
        }
    }
}
