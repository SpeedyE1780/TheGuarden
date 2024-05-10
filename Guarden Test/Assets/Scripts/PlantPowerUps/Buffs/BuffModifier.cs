using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    internal abstract class BuffModifier : ScriptableObject
    {
        private List<IBuff> buffed = new List<IBuff>();

        internal void AddAndApplyBuff(IBuff buff)
        {
            buffed.Add(buff);
            ApplyBuff(buff);
            buff.OnIBuffDetroyed += RemoveAndClearBuff;
        }

        internal void RemoveAndClearBuff(IBuff buff)
        {
            buffed.Remove(buff);
            RemoveBuff(buff);
            buff.OnIBuffDetroyed -= RemoveAndClearBuff;
        }

        internal abstract void ApplyBuff(IBuff buff);
        internal abstract void RemoveBuff(IBuff buff);

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
