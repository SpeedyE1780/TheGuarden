using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Behaviors
{
    /// <summary>
    /// Parent class of plants that alter behavior once IBehavior enters them
    /// </summary>
    internal sealed class PlantBehavior : PlantPowerUp
    {
        [SerializeField, Tooltip("Behavior Modifier")]
        private BehaviorModifier modifier;
        [SerializeField, Tooltip("Audio Source")]
        private AudioSource audioSource;

        /// <summary>
        /// Apply plant behavior to IBehavior
        /// </summary>
        /// <param name="IBehavior">IBehavior who entered plant trigger</param>
        private void ApplyBehavior(IBehavior behavior)
        {
            modifier.ApplyBehavior(behavior);
            if(!audioSource.isPlaying) 
            {
                audioSource.Play();
            }
            
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
