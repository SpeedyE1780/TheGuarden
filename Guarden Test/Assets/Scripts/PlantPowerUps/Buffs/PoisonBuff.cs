using System.Collections;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Plant Power Ups/Buffs/Damage")]
    internal class PoisonBuff : BuffModifier
    {
        [SerializeField, Tooltip("Damage per burst")]
        private int damage;
        [SerializeField, Tooltip("Interval between burst")]
        private float duration;

        private Coroutine poisonDamageRoutine;

        internal override void ApplyBuff(IBuff buff)
        {
            poisonDamageRoutine = buff.StartCoroutine(PoisonDamage(buff));
        }

        internal override void RemoveBuff(IBuff buff)
        {
            buff.StopCoroutine(poisonDamageRoutine);
        }

        private IEnumerator PoisonDamage(IBuff buff)
        {
            while (true)
            {
                GameLogger.LogInfo($"{buff.Agent.name} damaged {damage}", this, GameLogger.LogCategory.PlantPowerUp);
                buff.Health.Damage(damage);
                yield return new WaitForSeconds(duration);
            }
        }
    }
}
