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
        private static GameObject shed;

        [SerializeField, Tooltip("Autofilled. Animal Navmesh Agent")]
        private NavMeshAgent agent;
        [SerializeField, Tooltip("Autofilled. Animal rigidbody")]
        private Rigidbody rb;
        [SerializeField, Tooltip("Autofilled. Animal collider")]
        private Collider animalCollider;
        [SerializeField, Tooltip("Minimum distance before considering agent destination reached")]
        private float stoppingDistance = 0.75f;
        [SerializeField, Tooltip("Force Field gameobject activated when inside force field")]
        private GameObject forceField;

        private bool hiding = false;

        public bool InsideForceField { get; private set; }

        public Rigidbody Rigidbody => rb;
        public NavMeshAgent Agent => agent;
        public Collider Collider => animalCollider;

        private void Awake()
        {
            if (shed == null)
            {
                shed = GameObject.FindGameObjectWithTag(Tags.Shed);
            }
        }

        void Start()
        {
            agent.SetDestination(NavMeshSurfaceExtensions.GetPointOnSurface());
        }

        private void OnEnable()
        {
            DayLightCycle.OnDayStarted += ExitShed;
            DayLightCycle.OnNightStarted += HideInShed;
        }

        private void OnDisable()
        {
            DayLightCycle.OnDayStarted -= ExitShed;
            DayLightCycle.OnNightStarted -= HideInShed;
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

        private void OnCollisionEnter(Collision collision)
        {
            if (!enabled || !agent.enabled)
            {
                enabled = true;
                agent.enabled = true;
                agent.SetDestination(NavMeshSurfaceExtensions.GetPointOnSurface());
            }
        }

        private void ExitShed()
        {
            hiding = false;
        }

        private void HideInShed()
        {
            SetDestination(shed.transform.position);
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

        /// <summary>
        /// Set if the animal is in a force field or no
        /// </summary>
        /// <param name="active">True if inside force field</param>
        public void ToggleForceField(bool active)
        {
            InsideForceField = active;
            forceField.SetActive(active);
        }

#if UNITY_EDITOR
        internal void AutofillComponents()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            animalCollider = GetComponent<Collider>();
        }
#endif
    }
}
