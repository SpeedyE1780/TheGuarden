using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden
{
    public interface IBehavior
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        NavMeshAgent Agent { get; }
    }
}
