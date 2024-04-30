using System.Collections;
using UnityEngine.AI;

namespace TheGuarden
{
    public interface IBehavior
    {
        void StartCoroutine(IEnumerator coroutine);
        NavMeshAgent Agent { get; }
    }
}
