using UnityEngine;
using UnityEngine.AI;
using TheGuarden.PlantPowerUps;
using TheGuarden.Utility;

namespace TheGuarden.NPC
{
    /// <summary>
    /// Animal represents the animals wandering around the scene
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(Collider))]
    public class Animal : MonoBehaviour
    {
        [SerializeField, Tooltip("Autofilled. Animal Navmesh Agent")]
        private NavMeshAgent agent;
        [SerializeField, Tooltip("Autofilled. Animal rigidbody")]
        private Rigidbody rb;
        [SerializeField, Tooltip("Autofilled. Animal collider")]
        private Collider animalCollider;
        [SerializeField, Tooltip("Minimum distance before considering agent destination reached")]
        private float stoppingDistance = 0.75f;

        public bool InsideForceField { get; set; }

        public Rigidbody Rigidbody => rb;
        public NavMeshAgent Agent => agent;
        public Collider Collider => animalCollider;

        void Start()
        {
            agent.SetDestination(NavMeshSurfaceExtensions.GetPointOnSurface());
        }

        void Update()
        {
            if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
            {
                agent.SetDestination(NavMeshSurfaceExtensions.GetPointOnSurface());
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
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.PlantBuff))
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
                agent.SetDestination(NavMeshSurfaceExtensions.GetPointOnSurface());
            }
        }

        /// <summary>
        /// Set agent destination
        /// </summary>
        /// <param name="destination">Agent destination</param>
        internal void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        /// <summary>
        /// Pause Behavior when kidnapped by enemy
        /// </summary>
        public void PauseBehavior()
        {
            animalCollider.enabled = false;
            agent.enabled = false;
            enabled = false;
            rb.isKinematic = true;
        }

        /// <summary>
        /// Resume behavior when released by enemy
        /// </summary>
        public void ResumeBehavior()
        {
            animalCollider.enabled = true;
            agent.enabled = true;
            enabled = true;
            rb.isKinematic = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            animalCollider = GetComponent<Collider>();
        }
#endif
    }
}
