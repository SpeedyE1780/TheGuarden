using UnityEngine;
using TheGuarden.Utility;
using TheGuarden.NPC;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// RepelBehavior repels animal from it
    /// </summary>
    internal class RepelBehavior : PlantBehavior
    {
        [SerializeField, Tooltip("Minimum range animals are repelled")]
        private float minimumRange = 2.0f;
        [SerializeField, Tooltip("Maximum range animals are repelled")]
        private float maximumRange = 20.0f;

#if UNITY_EDITOR
        internal float MinimumRange => minimumRange;
        internal float MaximumRange => maximumRange;
#endif

        /// <summary>
        /// Repel animal from plant
        /// </summary>
        /// <param name="animal">Animal that entered trigger</param>
        protected override void ApplyBehavior(Animal animal)
        {
            GameLogger.LogInfo(animal.name + " Repeled", gameObject, GameLogger.LogCategory.PlantPowerUp);
            animal.SetDestination(GetDestination(animal.transform.position));
        }

        /// <summary>
        /// Get position between [minimumRange, maximumRange]
        /// </summary>
        /// <returns>Position between [minimumRange, maximumRange]</returns>
        private Vector3 GetDestination(Vector3 animalPosition)
        {
            Vector3 direction = animalPosition - transform.position;
            Vector3 repelVector = direction.normalized * Random.Range(minimumRange, maximumRange);
            return animalPosition + repelVector;
        }
    }
}
