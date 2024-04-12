using UnityEngine;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

[CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField]
    private int threshold;
    [SerializeField]
    private AchievementTracker tracker;

    private bool isCompleted = false;

    public void Initialize(AchivementTrackerDictionary achievementsProgress)
    {
        tracker.Initialize(achievementsProgress);
        isCompleted = tracker.Count >= threshold;
        GameLogger.LogInfo($"{name} is completed: {isCompleted}", this);

        tracker.OnValueChanged += OnProgress;
    }

    public void Deinitialize(AchivementTrackerDictionary achievementsProgress)
    {
        tracker.SaveProgress(achievementsProgress);
        tracker.OnValueChanged -= OnProgress;

#if UNITY_EDITOR
        tracker.Reset();
#endif
    }

    private void OnProgress(int value)
    {
        if (!isCompleted && value >= threshold)
        {
            isCompleted = true;
            GameLogger.LogInfo($"{name} is Completed", this);
        }
    }
}
