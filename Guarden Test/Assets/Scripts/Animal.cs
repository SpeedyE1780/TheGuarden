using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float stoppingDistance;

    private Vector3 GetNewDestination()
    {
        Vector3 destination = Random.insideUnitSphere * 20.0f;
        destination.y = 0;
        return destination;
    }

    void Start()
    {
        agent.SetDestination(GetNewDestination());
    }

    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < stoppingDistance)
        {
            agent.SetDestination(GetNewDestination());
        }
    }

    private void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
