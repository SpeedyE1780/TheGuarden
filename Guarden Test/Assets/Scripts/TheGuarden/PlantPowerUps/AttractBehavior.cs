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
        [SerializeField]
        private float attractionAreaRadius = 5.0f;

#if UNITY_EDITOR
        internal float AttractionAreaRadius => attractionAreaRadius;
#endif

        /// <summary>
        /// Attract animal towards plant
        /// </summary>
        /// <param name="animal">Animal that entered trigger</param>
        public override void ApplyBehavior(Animal animal)
        {
            GameLogger.LogInfo(animal.name + " Attracted", gameObject, GameLogger.LogCategory.PlantBehaviour);
            animal.SetDestination(GetDestination());
        }

        /// <summary>
        /// Get position inside attraction radius
        /// </summary>
        /// <returns>Position inside attraction radius</returns>
        private Vector3 GetDestination()
        {
            Vector3 destination = transform.position + Random.insideUnitSphere * attractionAreaRadius;
            destination.y = 0;
            return destination;
        }
    }
}
