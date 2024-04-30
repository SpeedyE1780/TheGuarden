using System.Collections;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.PlantPowerUps
{
    public interface IBuff
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(IEnumerator coroutine);
        NavMeshAgent Agent { get; }
        Health Health { get; }
    }
}
