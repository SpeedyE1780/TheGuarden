using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.Achievements
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement")]
    public class Achievement : ScriptableObject
    {
        [SerializeField]
        private int threshold;
        [SerializeField]
        internal AchievementTracker tracker;

        private bool isCompleted = false;

        internal void Initialize()
        {
            isCompleted = tracker.count >= threshold;
            GameLogger.LogInfo($"{name} is completed: {isCompleted}", this, GameLogger.LogCategory.Achievements);

            tracker.OnValueChanged += OnProgress;
        }

        internal void Deinitialize()
        {
            tracker.OnValueChanged -= OnProgress;
        }

        private void OnProgress(int value)
        {
            if (!isCompleted && value >= threshold)
            {
                isCompleted = true;
                GameLogger.LogInfo($"{name} is Completed", this, GameLogger.LogCategory.Achievements);
            }
        }
    } 
}
