using System.Collections;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Behaviors
{
    /// <summary>
    /// BehaviorModifier scriptable object that affects IBehaviors
    /// </summary>
    internal abstract class BehaviorModifier : ScriptableObject
    {
        /// <summary>
        /// Apply Behavior when IBehavior enter trigger
        /// </summary>
        /// <param name="behavior">IBehavior who entered the trigger</param>
        internal void ApplyBehavior(IBehavior behavior)
        {
            behavior.StartCoroutine(RunBehavior(behavior));
        }

        /// <summary>
        /// Run behavior
        /// </summary>
        /// <param name="behavior">IBehavior affected by modifier</param>
        /// <returns></returns>
        protected abstract IEnumerator RunBehavior(IBehavior behavior);
    }
}
