using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// Enemy is a State Machine that will patrol the scene and try to kidnap animals
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField, Tooltip("Speed at which enemy patrol the scene")]
        private float agentSpeed = 5.0f;
        [SerializeField, Tooltip("Minimum distance before destination is considered reached")]
        private float distanceThreshold = 3.0f;

        private EnemyPath path;

        private bool ReachedDestination => !agent.pathPending && agent.remainingDistance <= distanceThreshold;

#if UNITY_EDITOR
        internal NavMeshAgent Agent => agent;
#endif

        void Start()
        {
            StartCoroutine(Patrol());
        }

        /// <summary>
        /// Set enemy patrol path
        /// </summary>
        /// <param name="patrolPath">Enemy patrol path</param>
        internal void SetPath(EnemyPath patrolPath)
        {
            path = patrolPath;
        }

        /// <summary>
        /// Patrol State
        /// </summary>
        /// <returns></returns>
        private IEnumerator Patrol()
        {
            agent.SetDestination(path.CurrentPosition);
            agent.speed = agentSpeed;

            while (true)
            {
                if (ReachedDestination)
                {
                    path.CurrentIndex += 1;

                    if (path.ReachedEndOfPath)
                    {
                        Destroy(gameObject);
                        yield break;
                    }

                    agent.SetDestination(path.CurrentPosition);
                }

                yield return null;
            }
        }
    }
}
