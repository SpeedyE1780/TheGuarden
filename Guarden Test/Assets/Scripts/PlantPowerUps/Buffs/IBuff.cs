using System.Collections;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.PlantPowerUps.Buffs
{
    /// <summary>
    /// Interface representing classes affected by buffs
    /// </summary>
    public interface IBuff
    {
        delegate void OnIBuffDestroy(IBuff buff);

        NavMeshAgent Agent { get; }
        Health Health { get; }
        OnIBuffDestroy OnIBuffDetroyed { get; set; }

        /// <summary>
        /// Implemented by default by Monobehavior
        /// </summary>
        /// <param name="coroutine">Function to run</param>
        /// <returns></returns>
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
