using TheGuarden.NPC;

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
        public abstract void ApplyBehavior(Animal animal);
    }
}
