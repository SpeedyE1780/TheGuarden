using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Base class of all plant power ups
    /// </summary>
    internal abstract class PlantPowerUp : MonoBehaviour
    {
        [SerializeField, Tooltip("Range of power up")]
        protected float powerUpRange;
        [SerializeField, Tooltip("Collider used to check if animal entered/exited trigger")]
        private SphereCollider powerUpCollider;

#if UNITY_EDITOR
        internal float PowerUpRange => powerUpRange;

        private void OnValidate()
        {
            powerUpCollider = GetComponent<SphereCollider>();
            powerUpCollider.radius = powerUpRange;
        }
#endif
    }
}
