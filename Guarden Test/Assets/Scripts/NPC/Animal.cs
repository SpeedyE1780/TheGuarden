using UnityEngine;
using UnityEngine.AI;
using TheGuarden.Utility;

namespace TheGuarden.NPC
{
    /// <summary>
    /// Animal represents the animals wandering around the scene
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(Collider))]
    public class Animal : MonoBehaviour
    {
        internal static Transform Shed { get; set; }

        [SerializeField, Tooltip("Autofilled. Animal Navmesh Agent")]
        private NavMeshAgent agent;
        [SerializeField, Tooltip("Autofilled. Animal rigidbody")]
        private Rigidbody rb;
        [SerializeField, Tooltip("Minimum distance before considering agent destination reached")]
        private float stoppingDistance = 0.75f;
        [SerializeField, Tooltip("List of all spawned animals")]
        private AnimalSet spawnedAnimals;

        private bool hiding = false;

        void Start()
        {
            agent.SetDestination(NavMeshSurfaceExtensions.GetPointOnSurface());
            agent.stoppingDistance = stoppingDistance;
        }

        private void OnEnable()
        {
            spawnedAnimals.Add(this);
        }

        private void OnDisable()
        {
            spawnedAnimals.Remove(this);
        }

        void Update()
        {
            if (!agent.pathPending && agent.remainingDistance <= stoppingDistance && !hiding)
            {
                agent.SetDestination(NavMeshSurfaceExtensions.GetPointOnSurface());
            }
        }

        private void LateUpdate()
        {
            rb.velocity = agent.velocity;
        }

        public void ExitShed()
        {
            hiding = false;
        }

        public void HideInShed()
        {
            SetDestination(Shed.position);
            hiding = true;
        }

        /// <summary>
        /// Set agent destination
        /// </summary>
        /// <param name="destination">Agent destination</param>
        internal void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

#if UNITY_EDITOR
        internal void AutofillComponents()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }
#endif
    }
}
