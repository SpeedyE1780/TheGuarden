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
    [SerializeField]
    private Transform holdingPoint;
    private EnemyPath path;
    private Animal targetAnimal;

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

    private Animal DetectAnimal()
    {
        Collider[] animals = Physics.OverlapSphere(transform.position, detectionRadius, animalMask);

        foreach (Collider animalCollider in animals)
        {
            Animal animal = animalCollider.GetComponent<Animal>();

            if (!animal.InsideForceField)
            {
                return animal;
            }
        }

        return null;
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

            targetAnimal = DetectAnimal();

            if (targetAnimal != null)
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
        agent.SetDestination(targetAnimal.transform.position);

        while (targetAnimal != null && targetAnimal.Collider.enabled && !targetAnimal.InsideForceField)
        {
            if (ReachedDestination)
            {
                StartCoroutine(HoldAnimal());
                yield break;
            }

            agent.SetDestination(targetAnimal.transform.position);
            yield return null;
        }

        StartCoroutine(Patrol());
    }

    private IEnumerator HoldAnimal()
    {
        targetAnimal.PauseBehavior();
        agent.SetDestination(transform.position);

        while (targetAnimal.transform.position != holdingPoint.position)
        {
            targetAnimal.transform.position = Vector3.MoveTowards(targetAnimal.transform.position, holdingPoint.position, Time.deltaTime * 2);
            yield return null;
        }

        StartCoroutine(Escape());
    }

    private IEnumerator Escape()
    {
        agent.SetDestination(path.LastPosition);

        while (true)
        {
            targetAnimal.transform.position = holdingPoint.position;

            if (ReachedDestination)
            {
                Destroy(targetAnimal.gameObject);
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (targetAnimal != null && !targetAnimal.Collider.enabled)
        {
            targetAnimal.ResumeBehavior();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
