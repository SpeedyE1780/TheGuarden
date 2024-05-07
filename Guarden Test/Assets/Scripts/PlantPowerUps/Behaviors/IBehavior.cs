using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.PlantPowerUps
{
    public interface IBehavior
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void RewindPathProgress(int waypoints);
        NavMeshAgent Agent { get; }
    }
}
