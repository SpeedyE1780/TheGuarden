using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Achievements
{
    /// <summary>
    /// Scriptable Objects that listens to an AchievementGameEvent
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Listeners/<Achievement>")]
    internal class AchievementGameEventListenerSO : TGameEventListenerSO<Achievement> { }
}
