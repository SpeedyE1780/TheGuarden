using UnityEditor;
using TheGuarden.Utility.Editor;

namespace TheGuarden.UI.Editor
{
    internal class DateTextEditor
    {
        [MenuItem("CONTEXT/DateText/Set Date to Day 1")]
        internal static void AutofillTrackers(MenuCommand command)
        {
            DateText dateText = command.context as DateText;
            RecordEditorHistory.RecordHistory(dateText, $"Set {dateText.name} to day 1", dateText.SetDateToDayOne);
        }
    }
}
