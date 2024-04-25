using UnityEngine;
using TheGuarden.NPC;
using System.Collections.Generic;
using TheGuarden.Utility;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Parent class of plants that apply buff to animals inside trigger
    /// </summary>
    internal abstract class PlantBuff : PlantPowerUp
    {
        private List<Animal> buffedAnimals = new List<Animal>();

        /// <summary>
        /// Apply buff to animal
        /// </summary>
        /// <param name="animal">Animal staying in trigger</param>
        protected abstract void ApplyBuff(Animal animal);
        /// <summary>
        /// Remove buff to animal
        /// </summary>
        /// <param name="animal">Animal leaving trigger</param>
        protected abstract void RemoveBuff(Animal animal);

        /// <summary>
        /// Get animal from collider
        /// </summary>
        /// <param name="other">Object that entered power up trigger</param>
        /// <returns></returns>
        private Animal GetAnimal(Collider other)
        {
            Animal animal = other.GetComponent<Animal>();

            if (animal == null)
            {
                GameLogger.LogError($"{other.name} has no animal collider", this, GameLogger.LogCategory.PlantPowerUp);
            }

            return animal;
        }

        private void OnTriggerEnter(Collider other)
        {
            Animal animal = GetAnimal(other);

            if (animal == null)
            {
                return;
            }

            buffedAnimals.Add(animal);
        }

        private void OnTriggerStay(Collider other)
        {
            Animal animal = GetAnimal(other);

            if (animal == null)
            {
                return;
            }

            ApplyBuff(animal);
        }

        private void OnTriggerExit(Collider other)
        {
            Animal animal = GetAnimal(other);

            if (animal == null)
            {
                return;
            }

            RemoveBuff(animal);
            buffedAnimals.Remove(animal);
        }

        protected virtual void OnDisable()
        {
            GameLogger.LogInfo($"{name} picked up removing buff from affected objects", this, GameLogger.LogCategory.PlantPowerUp);

            foreach (Animal animal in buffedAnimals)
            {
                if (animal == null)
                {
                    continue;
                }

                RemoveBuff(animal);
            }

            buffedAnimals.Clear();
        }
    }
}
