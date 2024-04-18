using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TheGuarden.Utility;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private List<Achievement> achievements;
    [SerializeField]
    private List<AchievementTracker> achievementTrackers;

    private static readonly string AchievementDirectory = Application.streamingAssetsPath;
    private static readonly string AchievementPath = AchievementDirectory + "/Achievements.json";

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
        foreach (Achievement achievement in achievements)
        {
            achievementTrackers.Add(achievement.tracker);
        }
    }
#endif
}
