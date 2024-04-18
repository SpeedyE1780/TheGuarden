using System.Collections.Generic;
using UnityEngine;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

[CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement Tracker")]
internal class AchievementTracker : ScriptableObject
{
    internal delegate void ValueChanged(int value);
    internal int count = 0;

    internal event ValueChanged OnValueChanged;

    internal void Initialize(AchivementTrackerDictionary achievementsProgress)
    {
        count = achievementsProgress.GetValueOrDefault(name, 0);
    }

    internal void SaveProgress(AchivementTrackerDictionary achievementsProgress)
    {
        achievementsProgress.Add(name, count);
    }

    public void IncreaseCount(int amount)
    {
        count += amount;

        if (amount > 0)
        {
            OnValueChanged?.Invoke(count);
        }
    }
}
