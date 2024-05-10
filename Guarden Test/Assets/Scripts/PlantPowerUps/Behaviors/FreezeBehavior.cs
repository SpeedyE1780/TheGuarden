using System.Collections;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Behaviors
{
    /// <summary>
    /// FreezeBehavior will stop IBehavior movement for a short duration
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Plant Power Ups/Behaviors/Freeze")]
    internal class FreezeBehavior : BehaviorModifier
    {
        [SerializeField, Tooltip("Duration behavior is frozen")]
        private float duration = 3.0f;

        /// <summary>
        /// Freeze movement
        /// </summary>
        /// <param name="behavior">IBehavior who is frozen</param>
        /// <returns></returns>
        protected override IEnumerator RunBehavior(IBehavior behavior)
        {
            float speed = behavior.Agent.speed;
            behavior.Agent.speed = 0.0f;
            yield return new WaitForSeconds(duration);
            behavior.Agent.speed = speed;
        }
    }
}
