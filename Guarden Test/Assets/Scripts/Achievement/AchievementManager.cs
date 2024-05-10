using Newtonsoft.Json;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

namespace TheGuarden.Achievements
{
    /// <summary>
    /// AchievementManager keeps track of active achievements in scene
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Manager")]
    internal class AchievementManager : ScriptableObject
    {
        private static readonly string AchievementDirectory = Application.streamingAssetsPath;
        private static readonly string AchievementPath = AchievementDirectory + "/Achievements.json";

        [SerializeField, Tooltip("All achievements in the game")]
        private List<Achievement> allAchievements = new List<Achievement>();
        [SerializeField, Tooltip("Game Event called when an achievement is completed")]
        private AchievementGameEvent onAchievementCompleted;

        internal delegate void TrackerState(AchivementTrackerDictionary dictionary);
        internal static event TrackerState InitializeTrackers;
        internal static event TrackerState SaveTrackers;
        internal delegate void AchievementInitializer(AchievementGameEvent onCompleted);
        internal static event AchievementInitializer InitializeAchievements;
        internal delegate void AchievementDeinitializer();
        internal static event AchievementDeinitializer DeinitializeAchievements;

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

            InitializeTrackers.Invoke(achievementsProgress);
        }

        /// <summary>
        /// Read trackers value and save them to disk
        /// </summary>
        [System.Diagnostics.Conditional("THE_GUARDEN_SAVE_ACHIEVEMENTS")]
        private void SaveAchievementProgress()
        {
            GameLogger.LogInfo("Saving Achievements to file", this, GameLogger.LogCategory.Achievements);
            AchivementTrackerDictionary achievementsProgress = new AchivementTrackerDictionary();
            SaveTrackers.Invoke(achievementsProgress);
            string achievementJSON = JsonConvert.SerializeObject(achievementsProgress, Formatting.Indented);
            FileUtility.WriteFile(AchievementPath, achievementJSON);
        }

        /// <summary>
        /// Called from OnGameLoaded Event
        /// </summary>
        public void OnGameLoaded()
        {
            GameLogger.LogInfo("Achievement Manager Initializing", this, GameLogger.LogCategory.Achievements);
            LoadAchievementProgress();
            InitializeAchievements.Invoke(onAchievementCompleted);
        }

        /// <summary>
        /// Called from OnExitGameEvent
        /// </summary>
        public void OnExitGame()
        {
            GameLogger.LogInfo("Achievement Manager Saving", this, GameLogger.LogCategory.Achievements);
            SaveAchievementProgress();
            DeinitializeAchievements.Invoke();
        }

#if UNITY_EDITOR
        internal void FillAchievements(List<Achievement> achievementsAssets)
        {
            allAchievements.Clear();
            allAchievements.AddRange(achievementsAssets);
        }

#endif
    }
}
