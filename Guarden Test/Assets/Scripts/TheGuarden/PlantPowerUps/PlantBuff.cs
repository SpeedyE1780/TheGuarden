using TheGuarden.NPC;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Parent class of plants that apply buff to animals inside trigger
    /// </summary>
    internal abstract class PlantBuff : PlantPowerUp
    {
        /// <summary>
        /// Apply buff to animal
        /// </summary>
        /// <param name="animal">Animal staying in trigger</param>
        public abstract void ApplyBuff(Animal animal);
        /// <summary>
        /// Remove buff to animal
        /// </summary>
        /// <param name="animal">Animal leaving trigger</param>
        public abstract void RemoveBuff(Animal animal);
    }
}
