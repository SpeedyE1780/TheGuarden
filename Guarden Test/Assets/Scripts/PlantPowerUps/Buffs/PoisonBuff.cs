using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Buffs
{
    /// <summary>
    /// Poison Buff will apply damage periodically
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Plant Power Ups/Buffs/Damage")]
    internal class PoisonBuff : BuffModifier
    {
        [SerializeField, Tooltip("Damage per burst")]
        private float damage;
        [SerializeField, Tooltip("Interval between burst")]
        private float duration;

        private List<int> activeBuffs = new List<int>();

        /// <summary>
        /// Start Poison damage coroutine on buff
        /// </summary>
        /// <param name="buff">IBuff who entered the trigger</param>
        internal override void ApplyBuff(IBuff buff)
        {
            buff.StartCoroutine(PoisonDamage(buff));
            activeBuffs.Add(buff.Agent.GetInstanceID());
        }

        /// <summary>
        /// Remove buff from active list
        /// </summary>
        /// <param name="buff">IBuff who exited the trigger</param>
        internal override void RemoveBuff(IBuff buff)
        {
            int id = buff.Agent.GetInstanceID();
            activeBuffs.Remove(id);
            GameLogger.LogInfo($"{buff.Agent.name} poison buff removed", this, GameLogger.LogCategory.PlantPowerUp);
        }

        /// <summary>
        /// Apply poison damage while buff is in active list
        /// </summary>
        /// <param name="buff">Buff who is being poisoned</param>
        /// <returns></returns>
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
