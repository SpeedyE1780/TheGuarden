using UnityEngine;
using TheGuarden.Utility.Events;

namespace TheGuarden.Achievements
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game Events/Listeners/<Achievement>")]
    internal class AchievementGameEventListenerSO : TGameEventListenerSO<Achievement> { }
}
