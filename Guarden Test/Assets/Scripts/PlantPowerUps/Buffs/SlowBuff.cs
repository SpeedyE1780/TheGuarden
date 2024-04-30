using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Plant Power Ups/Buffs/Speed Modifier")]
    internal class SlowBuff : BuffModifier
    {
        [SerializeField, Tooltip("Speed modifier applied to agent")]
        private float speedFactor = 0.5f;

        internal override void ApplyBuff(IBuff buff)
        {
            buff.Agent.speed *= speedFactor;
        }

        internal override void RemoveBuff(IBuff buff)
        {
            buff.Agent.speed /= speedFactor;
        }
    }
}
