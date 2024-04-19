using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheGuarden.Achievements.Editor
{
    internal class AchievementManagerEditor
    {
        [MenuItem("CONTEXT/AchievementManager/Autofill Trackers")]
        internal static void AutofillTrackers(MenuCommand command)
        {
            AchievementManager achievementManager = command.context as AchievementManager;
            achievementManager.FillTrackers();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
