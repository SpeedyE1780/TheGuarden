using UnityEngine;
using TheGuarden.Utility;
using TheGuarden.NPC;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// ForceFieldBuff applies protection buff to animal and prevents enemy from entering it
    /// </summary>
    internal class ForceFieldBuff : PlantBuff
    {
#if UNITY_EDITOR
        [SerializeField]
        private SphereCollider navMeshModifierCollider;
#endif

        private void OnEnable()
        {
            EnemyNavMeshBaker.BakeNavMesh();
        }

        private void OnDisable()
        {
            EnemyNavMeshBaker.BakeNavMesh();
        }

        /// <summary>
        /// Set animal inside force field
        /// </summary>
        /// <param name="animal">Animal inside force field</param>
        public override void ApplyBuff(Animal animal)
        {
            GameLogger.LogInfo(animal.name + " in force field", gameObject, GameLogger.LogCategory.PlantBehaviour);
            animal.InsideForceField = true;
        }

        /// <summary>
        /// Set animal outside force field
        /// </summary>
        /// <param name="animal">Animal outside force field</param>
        public override void RemoveBuff(Animal animal)
        {
            GameLogger.LogInfo(animal.name + " out of force field", gameObject, GameLogger.LogCategory.PlantBehaviour);
            animal.InsideForceField = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (navMeshModifierCollider != null)
            {
                navMeshModifierCollider.radius = powerUpRange;
            }
        }
#endif
    }
}
