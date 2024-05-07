using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.Achievements
{
    /// <summary>
    /// Achievement represents an in game achievement
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement")]
    public class Achievement : ScriptableObject
    {
        [SerializeField, Tooltip("Value needed to unlock achievement")]
        private int threshold;
        [SerializeField, Tooltip("Achievement description")]
        private string description;
        [SerializeField, Tooltip("Tracker used to keep track of event in game")]
        internal AchievementTracker tracker;

        private bool isCompleted = false;
        private AchievementGameEvent onAchievementCompleted;

        public string Description => description;

        /// <summary>
        /// Check if achievement is completed and subscribe to tracker's value changed
        /// </summary>
        internal void Initialize(AchievementGameEvent onCompleted)
        {
            isCompleted = tracker.count >= threshold;
            GameLogger.LogInfo($"{name} is completed: {isCompleted}", this, GameLogger.LogCategory.Achievements);
            onAchievementCompleted = onCompleted;

            if (!isCompleted)
            {
                tracker.OnValueChanged += OnProgress;
            }
        }

        /// <summary>
        /// Unsubscribe from tracker's value changed
        /// </summary>
        internal void Deinitialize()
        {
            tracker.OnValueChanged -= OnProgress;
        }

        /// <summary>
        /// Check if achievement is completed when tracker values changes
        /// </summary>
        /// <param name="value">New Tracker value</param>
        private void OnProgress(int value)
        {
            if (!isCompleted && value >= threshold)
            {
                isCompleted = true;
                onAchievementCompleted.Raise(this);
                GameLogger.LogInfo($"{name} is Completed", this, GameLogger.LogCategory.Achievements);
                tracker.OnValueChanged -= OnProgress;
            }
        }
    }
}
