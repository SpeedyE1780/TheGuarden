using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    [CreateAssetMenu(menuName ="Scriptable Objects/Plant Power Ups/Config")]
    internal class PowerUpConfiguration : ScriptableObject
    {
        [SerializeField, Tooltip("Range of power up")]
        internal float powerUpRange;
        [SerializeField, Tooltip("Layers affected by power up")]
        internal LayerMask powerUpMask;
    }
}
