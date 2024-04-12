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
    private Collider animalCollider;
    [SerializeField]
    private float stoppingDistance;

    public bool InsideForceField { get; set; }

    public Rigidbody Rigidbody => rb;
    public NavMeshAgent Agent => agent;
    public Collider Collider => animalCollider;

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
        if (other.CompareTag(Tags.PlantBehavior))
        {
            other.GetComponent<PlantBehavior>().ApplyBehavior(this);
        }
        else if (other.CompareTag(Tags.PlantBuff))
        {
            other.GetComponent<PlantBuff>().ApplyBuff(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.PlantBuff))
        {
            other.GetComponent<PlantBuff>().RemoveBuff(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled || !agent.enabled)
        {
            enabled = true;
            agent.enabled = true;
            agent.SetDestination(GetNewDestination());
        }
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void PauseBehavior()
    {
        animalCollider.enabled = false;
        agent.enabled = false;
        enabled = false;
        rb.isKinematic = true;
    }

    public void ResumeBehavior()
    {
        animalCollider.enabled = true;
        agent.enabled = true;
        enabled = true;
        rb.isKinematic = false;
    }

    private void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animalCollider = GetComponent<Collider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = InsideForceField ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
