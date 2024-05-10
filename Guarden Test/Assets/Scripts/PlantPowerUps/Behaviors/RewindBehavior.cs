using System.Collections;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Behaviors
{
    /// <summary>
    /// Rewind Behavior makes the IBehavior rewind its path
    /// </summary>
    [CreateAssetMenu(menuName ="Scriptable Objects/Plant Power Ups/Behaviors/Return")]
    internal class RewindBehavior : BehaviorModifier
    {
        [SerializeField, Tooltip("Number of waypoints enemy should go back to")]
        private int count = 2;

        /// <summary>
        /// Make the IBehavior rewind its path
        /// </summary>
        /// <param name="behavior">IBehavior that will rewind its path</param>
        /// <returns></returns>
        protected override IEnumerator RunBehavior(IBehavior behavior)
        {
            behavior.RewindPathProgress(count);
            yield break;
        }
    }
}
