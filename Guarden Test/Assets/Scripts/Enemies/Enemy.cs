using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float patrolSpeed = 5.0f;
    [SerializeField]
    private float chaseSpeed = 7.0f;
    [SerializeField]
    private float distanceThreshold = 3.0f;
    [SerializeField]
    private float detectionRadius = 3.0f;
    [SerializeField]
    private LayerMask animalMask;
    private EnemyPath path;
    private Transform target;

    public EnemyPath Path
    {
        get
        {
            return path;
        }
        set
        {
            path = value;
        }
    }

    private bool ReachedDestination => !agent.pathPending && agent.remainingDistance <= distanceThreshold;

    void Start()
    {
        StartCoroutine(Patrol());
    }

    private Transform DetectAnimal()
    {
        Collider[] animals = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, animals, animalMask);

        return animals[0] != null ? animals[0].transform : null;
    }

    private IEnumerator Patrol()
    {
        agent.SetDestination(path.CurrentPosition);
        agent.speed = patrolSpeed;

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

            target = DetectAnimal();

            if (target != null)
            {
                StartCoroutine(Chase());
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(target.position);

        while (target != null)
        {
            if (ReachedDestination)
            {
                Destroy(target.gameObject);
                StartCoroutine(Escape());
                yield break;
            }

            agent.SetDestination(target.position);
            yield return null;
        }

        StartCoroutine(Patrol());
    }

    private IEnumerator Escape()
    {
        agent.SetDestination(path.LastPosition);

        while (true)
        {
            if (ReachedDestination)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
