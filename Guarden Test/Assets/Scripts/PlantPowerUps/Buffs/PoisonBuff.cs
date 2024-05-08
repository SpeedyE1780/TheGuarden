using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Plant Power Ups/Buffs/Damage")]
    internal class PoisonBuff : BuffModifier
    {
        [SerializeField, Tooltip("Damage per burst")]
        private float damage;
        [SerializeField, Tooltip("Interval between burst")]
        private float duration;

        private List<int> activeBuffs = new List<int>();

        internal override void ApplyBuff(IBuff buff)
        {
            buff.StartCoroutine(PoisonDamage(buff));
            activeBuffs.Add(buff.Agent.GetInstanceID());
        }

        internal override void RemoveBuff(IBuff buff)
        {
            int id = buff.Agent.GetInstanceID();
            activeBuffs.Remove(id);
            GameLogger.LogInfo($"{buff.Agent.name} poison buff removed", this, GameLogger.LogCategory.PlantPowerUp);
        }

        private IEnumerator PoisonDamage(IBuff buff)
        {
            int id = buff.Agent.GetInstanceID();
            GameLogger.LogInfo($"{buff.Agent.name} poison buff started", this, GameLogger.LogCategory.PlantPowerUp);

            while (activeBuffs.Contains(id))
            {
                GameLogger.LogInfo($"{buff.Agent.name} took {damage} poison damage", this, GameLogger.LogCategory.PlantPowerUp);
                buff.Health.Damage(damage);
                yield return new WaitForSeconds(duration);
            }

            GameLogger.LogInfo($"{buff.Agent.name} poison buff ended", this, GameLogger.LogCategory.PlantPowerUp);
            activeBuffs.Remove(id);
        }
    }
}
