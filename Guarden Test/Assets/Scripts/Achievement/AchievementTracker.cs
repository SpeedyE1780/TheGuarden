using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

using AchivementTrackerDictionary = System.Collections.Generic.Dictionary<string, int>;

namespace TheGuarden.Achievements
{
    /// <summary>
    /// AchievementTracker is used to keep track of an event count in game
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Tracker")]
    internal class AchievementTracker : ScriptableObject
    {
        internal delegate void ValueChanged(int value);
        internal int count = 0;

        internal event ValueChanged OnValueChanged;

        private void OnEnable()
        {
            AchievementManager.InitializeTrackers += Initialize;
            AchievementManager.SaveTrackers += SaveProgress;
        }

        private void OnDisable()
        {
            AchievementManager.InitializeTrackers -= Initialize;
            AchievementManager.SaveTrackers -= SaveProgress;
        }

        /// <summary>
        /// Initialize count from save file
        /// </summary>
        /// <param name="achievementsProgress">Dictionary containing all trackers saved values</param>
        private void Initialize(AchivementTrackerDictionary achievementsProgress)
        {
            count = achievementsProgress.GetValueOrDefault(name, 0);
            GameLogger.LogInfo($"{name} initialized with count: {count}", this, GameLogger.LogCategory.Achievements);
        }

        /// <summary>
        /// Save final count to save file
        /// </summary>
        /// <param name="achievementsProgress">Dictionary containing all trackers new values</param>
        private void SaveProgress(AchivementTrackerDictionary achievementsProgress)
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
