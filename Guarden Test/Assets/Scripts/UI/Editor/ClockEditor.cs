using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.UI.Editor
{
    internal class ClockEditor
    {
        [MenuItem("CONTEXT/Clock/Autofill GameTime")]
        internal static void AutofillGameTime(MenuCommand command)
        {
            Clock clock = command.context as Clock;
            RecordEditorHistory.RecordHistory(clock, $"Set {clock.name} GameTime", clock.AutofillGameTime);
        }
    }
}
