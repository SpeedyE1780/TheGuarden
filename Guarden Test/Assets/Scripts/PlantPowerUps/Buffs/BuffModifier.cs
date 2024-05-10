using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Buffs
{
    /// <summary>
    /// BuffModifier scriptable object
    /// </summary>
    internal abstract class BuffModifier : ScriptableObject
    {
        private List<IBuff> buffed = new List<IBuff>();

        /// <summary>
        /// Add buff to list and apply buff effect
        /// </summary>
        /// <param name="buff">IBuff who is gettig buffed</param>
        internal void AddAndApplyBuff(IBuff buff)
        {
            buffed.Add(buff);
            ApplyBuff(buff);
            buff.OnIBuffDetroyed += RemoveAndClearBuff;
        }

        /// <summary>
        /// Remove buff from list and clear buff effect
        /// </summary>
        /// <param name="buff">IBuff who exited buff range</param>
        internal void RemoveAndClearBuff(IBuff buff)
        {
            buffed.Remove(buff);
            RemoveBuff(buff);
            buff.OnIBuffDetroyed -= RemoveAndClearBuff;
        }

        /// <summary>
        /// Apply buff effect
        /// </summary>
        /// <param name="buff">IBuff receiving buff effect</param>
        internal abstract void ApplyBuff(IBuff buff);

        /// <summary>
        /// Clear buff effect
        /// </summary>
        /// <param name="buff">IBuff who exited buff range</param>
        internal abstract void RemoveBuff(IBuff buff);

        /// <summary>
        /// Clear all buffs and empty buffed list
        /// </summary>
        internal void RemoveAllBuff()
        {
            foreach (IBuff buff in buffed)
            {
                if (buff != null)
                {
                    RemoveBuff(buff);
                }
            }

            buffed.Clear();
        }
    }
}
