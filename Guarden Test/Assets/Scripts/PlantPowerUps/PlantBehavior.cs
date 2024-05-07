using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Parent class of plants that alter behavior once IBehavior enters them
    /// </summary>
    internal sealed class PlantBehavior : PlantPowerUp
    {
        [SerializeField, Tooltip("Behavior Modifier")]
        private BehaviorModifier modifier;

        /// <summary>
        /// Apply plant behavior to IBehavior
        /// </summary>
        /// <param name="IBehavior">IBehavior who entered plant trigger</param>
        private void ApplyBehavior(IBehavior behavior)
        {
            modifier.ApplyBehavior(behavior);
        }

        private void OnTriggerEnter(Collider other)
        {
            IBehavior behavior = other.GetComponent<IBehavior>();

            if (behavior == null)
            {
                GameLogger.LogWarning($"{other.name} has no IBehavior component", this, GameLogger.LogCategory.PlantPowerUp);
                return;
            }

            ApplyBehavior(behavior);
        }
    }
}
