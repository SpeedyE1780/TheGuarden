using UnityEngine;
using TheGuarden.Utility;
using TheGuarden.NPC;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// AttractBehavior attracts animal towards it
    /// </summary>
    internal class AttractBehavior : PlantBehavior
    {
        [SerializeField, Tooltip("Closest point to mushroom")]
        private float minimumRange = 5.0f;
        [SerializeField, Tooltip("Furthest point from mushroom")]
        private float maximumRange = 7.5f;

#if UNITY_EDITOR
        internal float MinimumRange => minimumRange;
        internal float MaximumRange => maximumRange;
#endif

        /// <summary>
        /// Attract animal towards plant
        /// </summary>
        /// <param name="animal">Animal that entered trigger</param>
        protected override void ApplyBehavior(Animal animal)
        {
            GameLogger.LogInfo(animal.name + " Attracted", gameObject, GameLogger.LogCategory.PlantPowerUp);
            animal.SetDestination(GetDestination(animal.transform.position));
        }

        /// <summary>
        /// Get position between [minimumRange, maximumRange]
        /// </summary>
        /// <returns>Position between [minimumRange, maximumRange]</returns>
        private Vector3 GetDestination(Vector3 animalPosition)
        {
            Vector3 direction = transform.position - animalPosition;
            Vector3 attractionVector = direction.normalized * Random.Range(minimumRange, maximumRange);
            return animalPosition + attractionVector;
        }
    }
}
