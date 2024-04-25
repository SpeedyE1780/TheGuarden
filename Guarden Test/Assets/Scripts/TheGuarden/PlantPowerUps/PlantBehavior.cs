using UnityEngine;
using TheGuarden.NPC;
using TheGuarden.Utility;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Parent class of plants that alter behavior once animal enters them
    /// </summary>
    internal abstract class PlantBehavior : PlantPowerUp
    {
        /// <summary>
        /// Apply plant behavior to animal
        /// </summary>
        /// <param name="animal">Animal who entered plant trigger</param>
        protected abstract void ApplyBehavior(Animal animal);

        private void OnTriggerEnter(Collider other)
        {
            Animal animal = other.GetComponent<Animal>();

            if (animal == null)
            {
                GameLogger.LogError($"{other.name} has no animal component", this, GameLogger.LogCategory.PlantPowerUp);
            }

            ApplyBehavior(animal);
        }
    }
}
