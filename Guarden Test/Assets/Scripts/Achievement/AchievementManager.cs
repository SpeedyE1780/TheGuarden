using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private List<Achievement> achievements;

    private static readonly string AchievementDirectory = Application.streamingAssetsPath;
    private static readonly string AchievementPath = AchievementDirectory + "/Achievements.json";

    private void Start()
    {
        string json = FileManager.ReadFile(AchievementPath);
        AchivementTrackerDictionary achievementsProgress = !string.IsNullOrWhiteSpace(json) ? 
            JsonConvert.DeserializeObject<AchivementTrackerDictionary>(json) :
            new AchivementTrackerDictionary();

        foreach (Achievement achievement in achievements)
        {
            achievement.Initialize(achievementsProgress);
        }
    }

    private void OnDestroy()
    {
        AchivementTrackerDictionary achievementsProgress = new AchivementTrackerDictionary();

        foreach (Achievement achievement in achievements)
        {
            achievement.Deinitialize(achievementsProgress);
        }

        string achievementJSON = JsonConvert.SerializeObject(achievementsProgress, Formatting.Indented);

        FileManager.WriteFile(AchievementPath, achievementJSON);
    }
}
