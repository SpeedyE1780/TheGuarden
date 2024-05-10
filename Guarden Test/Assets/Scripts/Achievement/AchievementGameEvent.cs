using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Achievements
{
    /// <summary>
    /// Game Event that passes the completed achievement as an argument
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Events/<Achievement>")]
    internal class AchievementGameEvent : TGameEvent<Achievement> { }
}
