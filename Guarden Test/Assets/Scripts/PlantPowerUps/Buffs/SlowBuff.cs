using UnityEngine;

namespace TheGuarden.PlantPowerUps.Buffs
{
    /// <summary>
    /// Slow IBuff while in range
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Plant Power Ups/Buffs/Speed Modifier")]
    internal class SlowBuff : BuffModifier
    {
        [SerializeField, Tooltip("Speed modifier applied to agent")]
        private float speedFactor = 0.5f;

    /// <summary>
    /// Reduce IBuff speed
    /// </summary>
    /// <param name="buff">IBuff who entered trigger</param>
    internal override void ApplyBuff(IBuff buff)
        {
            buff.Agent.speed *= speedFactor;
        }

        /// <summary>
        /// Restore IBuff speed
        /// </summary>
        /// <param name="buff">IBuff who exited trigger</param>
        internal override void RemoveBuff(IBuff buff)
        {
            buff.Agent.speed /= speedFactor;
        }
    }
}
