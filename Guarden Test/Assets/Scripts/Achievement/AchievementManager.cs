using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TheGuarden.Utility;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

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

        private static readonly string AchievementDirectory = Application.streamingAssetsPath;
        private static readonly string AchievementPath = AchievementDirectory + "/Achievements.json";

        /// <summary>
        /// Read save file and set tracker values to saved value or 0
        /// </summary>
        private void Start()
        {
            string json = FileUtility.ReadFile(AchievementPath);
            AchivementTrackerDictionary achievementsProgress = !string.IsNullOrWhiteSpace(json) ?
                JsonConvert.DeserializeObject<AchivementTrackerDictionary>(json) :
                new AchivementTrackerDictionary();

            foreach (AchievementTracker tracker in achievementTrackers)
            {
                tracker.Initialize(achievementsProgress);
            }

            foreach (Achievement achievement in achievements)
            {
                achievement.Initialize();
            }
        }

        /// <summary>
        /// Read trackers value and save them to disk
        /// </summary>
        private void OnDestroy()
        {
            AchivementTrackerDictionary achievementsProgress = new AchivementTrackerDictionary();

            foreach (AchievementTracker tracker in achievementTrackers)
            {
                tracker.SaveProgress(achievementsProgress);
            }

            foreach (Achievement achievement in achievements)
            {
                achievement.Deinitialize();
            }

            string achievementJSON = JsonConvert.SerializeObject(achievementsProgress, Formatting.Indented);

            FileUtility.WriteFile(AchievementPath, achievementJSON);
        }

#if UNITY_EDITOR
        private void OnValidate()
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
