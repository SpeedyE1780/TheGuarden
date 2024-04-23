using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TheGuarden.Utility;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;
using UnityEngine.Events;

namespace TheGuarden.Achievements
{
    /// <summary>
    /// AchievementManager keeps track of active achievements in scene
    /// </summary>
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Active achievements in scene")]
        private List<Achievement> achievements;
        [SerializeField, Tooltip("Autofilled from achievements list. All active trackers in scene")]
        private List<AchievementTracker> achievementTrackers;

        public UnityEvent<Achievement> OnAchievementCompleted;

        private static readonly string AchievementDirectory = Application.streamingAssetsPath;
        private static readonly string AchievementPath = AchievementDirectory + "/Achievements.json";

        /// <summary>
        /// Read save file and set tracker values to saved value or 0
        /// </summary>
        private void LoadAchievementProgress()
        {
#if THE_GUARDEN_SAVE_ACHIEVEMENTS
            GameLogger.LogInfo("Loading Achievements from file", this, GameLogger.LogCategory.Achievements);
            string json = FileUtility.ReadFile(AchievementPath);
            AchivementTrackerDictionary achievementsProgress = !string.IsNullOrWhiteSpace(json) ?
                JsonConvert.DeserializeObject<AchivementTrackerDictionary>(json) :
                new AchivementTrackerDictionary();
#else
            GameLogger.LogInfo("Creating empty achievements dictionary", this, GameLogger.LogCategory.Achievements);
            AchivementTrackerDictionary achievementsProgress = new AchivementTrackerDictionary();
#endif

            foreach (AchievementTracker tracker in achievementTrackers)
            {
                tracker.Initialize(achievementsProgress);
            }
        }

        /// <summary>
        /// Read trackers value and save them to disk
        /// </summary>
        [System.Diagnostics.Conditional("THE_GUARDEN_SAVE_ACHIEVEMENTS")]
        private void SaveAchievementProgress()
        {
            GameLogger.LogInfo("Saving Achievements to file", this, GameLogger.LogCategory.Achievements);

            AchivementTrackerDictionary achievementsProgress = new AchivementTrackerDictionary();

            foreach (AchievementTracker tracker in achievementTrackers)
            {
                tracker.SaveProgress(achievementsProgress);
            }

            string achievementJSON = JsonConvert.SerializeObject(achievementsProgress, Formatting.Indented);
            FileUtility.WriteFile(AchievementPath, achievementJSON);
        }

        private void Start()
        {
            LoadAchievementProgress();

            foreach (Achievement achievement in achievements)
            {
                achievement.Initialize(OnAchievementCompleted);
            }
        }

        private void OnDestroy()
        {
            SaveAchievementProgress();

            foreach (Achievement achievement in achievements)
            {
                achievement.Deinitialize();
            }
        }

#if UNITY_EDITOR
        internal void FillTrackers()
        {
            achievementTrackers.Clear();

            foreach (Achievement achievement in achievements)
            {
                if (!achievementTrackers.Contains(achievement.tracker))
                {
                    achievementTrackers.Add(achievement.tracker);
                }
            }
        }
#endif
    }
}
