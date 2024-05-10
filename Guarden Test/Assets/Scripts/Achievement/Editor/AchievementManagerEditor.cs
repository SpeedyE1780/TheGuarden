using System.Collections.Generic;
using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Achievements.Editor
{
    internal class AchievementManagerEditor
    {
        private static List<Achievement> GetAchievementsAssets()
        {
            List<Achievement> list = new List<Achievement>();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(Achievement)}");

            foreach (var t in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<Achievement>(assetPath);

                if (asset != null)
                {
                    list.Add(asset);
                }
            }

            return list;
        }

        [MenuItem("CONTEXT/AchievementManager/Autofill Achievements")]
        internal static void AutofillTrackers(MenuCommand command)
        {
            AchievementManager achievementManager = command.context as AchievementManager;
            RecordEditorHistory.RecordHistory(achievementManager, $"Fill {achievementManager.name} active trackers", () => achievementManager.FillAchievements(GetAchievementsAssets()));
        }

    }
}
