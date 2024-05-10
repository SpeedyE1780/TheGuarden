using System.Collections;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.PlantPowerUps
{
    public interface IBuff
    {
        delegate void OnIBuffDestroy(IBuff buff);

        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
        NavMeshAgent Agent { get; }
        Health Health { get; }
        OnIBuffDestroy OnIBuffDetroyed { get; set; }
    }
}
