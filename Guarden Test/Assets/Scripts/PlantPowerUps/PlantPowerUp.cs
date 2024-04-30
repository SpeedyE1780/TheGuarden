using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Base class of all plant power ups
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public abstract class PlantPowerUp : MonoBehaviour
    {
        [SerializeField, Tooltip("Power up configuration")]
        private PowerUpConfiguration configuration;
        [SerializeField, Tooltip("Collider used to check if animal entered/exited trigger")]
        private SphereCollider powerUpCollider;

        protected float Range => configuration.powerUpRange;

        private void Start()
        {
            powerUpCollider.includeLayers = configuration.powerUpMask;
            powerUpCollider.excludeLayers = ~configuration.powerUpMask;
            powerUpCollider.radius = configuration.powerUpRange;
        }

#if UNITY_EDITOR
        internal float PowerUpRange => configuration.powerUpRange;
#endif
    }
}
