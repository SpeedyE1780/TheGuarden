using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.PlantPowerUps.Behaviors
{
    /// <summary>
    /// Interface representing classes affected by behaviors
    /// </summary>
    public interface IBehavior
    {
        NavMeshAgent Agent { get; }

        /// <summary>
        /// Implemented by default by Monobehavior
        /// </summary>
        /// <param name="coroutine">Function to run</param>
        /// <returns></returns>
        Coroutine StartCoroutine(IEnumerator coroutine);

        /// <summary>
        /// Move IBehavior backward 
        /// </summary>
        /// <param name="waypoints">How many waypoints should behavior go back</param>
        void RewindPathProgress(int waypoints);
    }
}
