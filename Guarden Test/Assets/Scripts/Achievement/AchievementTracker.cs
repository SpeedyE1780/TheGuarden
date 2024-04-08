using System.Collections.Generic;
using UnityEngine;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

[CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement Tracker")]
public class AchievementTracker : ScriptableObject
{
    public delegate void ValueChanged(int value);
    private int count = 0;
    private bool initialized = false;
    private bool saved = false;

    public event ValueChanged OnValueChanged;

    public int Count => count;

    public void Initialize(AchivementTrackerDictionary achievementsProgress)
    {
        if (!initialized)
        {
            count = achievementsProgress.GetValueOrDefault(name, 0);
            initialized = true;
        }
    }

    public void SaveProgress(AchivementTrackerDictionary achievementsProgress)
    {
        if (!saved)
        {
            achievementsProgress.Add(name, count);
            saved = true;
        }
    }

    public void IncreaseCount(int amount)
    {
        count += amount;

        if (amount > 0)
        {
            OnValueChanged?.Invoke(count);
        }
    }

    public void Reset()
    {
        count = 0;
        initialized = false;
        saved = false;
    }
}
