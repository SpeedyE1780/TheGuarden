using System.Collections.Generic;
using UnityEngine;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

namespace TheGuarden.Achievements
{
    /// <summary>
    /// AchievementTracker is used to keep track of an event count in game
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement Tracker")]
    internal class AchievementTracker : ScriptableObject
    {
        internal delegate void ValueChanged(int value);
        internal int count = 0;

        internal event ValueChanged OnValueChanged;

        /// <summary>
        /// Initialize count from save file
        /// </summary>
        /// <param name="achievementsProgress">Dictionary containing all trackers saved values</param>
        internal void Initialize(AchivementTrackerDictionary achievementsProgress)
        {
            count = achievementsProgress.GetValueOrDefault(name, 0);
        }

        /// <summary>
        /// Save final count to save file
        /// </summary>
        /// <param name="achievementsProgress">Dictionary containing all trackers new values</param>
        internal void SaveProgress(AchivementTrackerDictionary achievementsProgress)
        {
            achievementsProgress.Add(name, count);
        }

        /// <summary>
        /// Increase tracker count after event occurs in game
        /// </summary>
        /// <param name="amount">Amount of time event occured</param>
        public void IncreaseCount(int amount)
        {
            count += amount;

            if (amount > 0)
            {
                OnValueChanged?.Invoke(count);
            }
        }

        /// <summary>
        /// Decrease tracker count after event occurs in game
        /// </summary>
        /// <param name="amount">Amount of time event occured</param>
        public void DecreaseCount(int amount)
        {
            count -= amount;

            if (amount > 0)
            {
                OnValueChanged?.Invoke(count);
            }
        }

        /// <summary>
        /// Reset tracker count after event occurs in game
        /// </summary>
        public void ResetCount()
        {
            count = 0;
            OnValueChanged?.Invoke(count);
        }
    }
}
