using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Base class of all plant power ups
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    internal abstract class PlantPowerUp : MonoBehaviour
    {
        [SerializeField, Tooltip("Range of power up")]
        protected float powerUpRange;
        [SerializeField, Tooltip("Collider used to check if animal entered/exited trigger")]
        private SphereCollider powerUpCollider;
        [SerializeField, Tooltip("Layers affected by power up")]
        private LayerMask powerUpMask;

        private void Start()
        {
            powerUpCollider.includeLayers = powerUpMask;
            powerUpCollider.excludeLayers = ~powerUpMask;
        }

#if UNITY_EDITOR
        internal float PowerUpRange => powerUpRange;
#endif
    }
}
