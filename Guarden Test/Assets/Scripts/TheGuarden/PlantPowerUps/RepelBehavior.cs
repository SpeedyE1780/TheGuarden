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
        [SerializeField, Tooltip("Max range animals are repelled")]
        private float repelRange;
        [SerializeField, Tooltip("Minimum range animals are repelled")]
        private float minimumRange;

        /// <summary>
        /// Repel animal from plant
        /// </summary>
        /// <param name="animal">Animal that entered trigger</param>
        public override void ApplyBehavior(Animal animal)
        {
            GameLogger.LogInfo(animal.name + " Repeled", gameObject, GameLogger.LogCategory.PlantBehaviour);
            animal.SetDestination(GetDestination(animal.transform.position));
        }

        /// <summary>
        /// Get position between [minimumRange, repelRange]
        /// </summary>
        /// <returns>Position between [minimumRange, repelRange]</returns>
        private Vector3 GetDestination(Vector3 animalPosition)
        {
            Vector3 direction = animalPosition - transform.position;
            Vector3 destination = direction.normalized * Random.Range(minimumRange, repelRange);
            return destination;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, powerUpRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, repelRange);
            Gizmos.DrawWireSphere(transform.position, minimumRange + powerUpRange);
        }
#endif
    }
}
