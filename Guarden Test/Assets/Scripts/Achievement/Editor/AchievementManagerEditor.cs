using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Achievements.Editor
{
    internal class AchievementManagerEditor
    {
        [MenuItem("CONTEXT/AchievementManager/Autofill Trackers")]
        internal static void AutofillTrackers(MenuCommand command)
        {
            AchievementManager achievementManager = command.context as AchievementManager;
            RecordEditorHistory.RecordHistory(achievementManager, $"Fill {achievementManager.name} active trackers", achievementManager.FillTrackers);
        }
    }
}
