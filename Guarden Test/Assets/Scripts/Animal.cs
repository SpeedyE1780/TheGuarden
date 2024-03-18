using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Rigidbody rb;
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
        if (!agent.pathPending && agent.remainingDistance < stoppingDistance)
        {
            agent.SetDestination(GetNewDestination());
        }
    }

    private void LateUpdate()
    {
        rb.velocity = agent.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlantBehavior"))
        {
            other.GetComponent<PlantBehavior>().ApplyBehavior(this);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
}
