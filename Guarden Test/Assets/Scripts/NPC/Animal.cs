using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.NPC
{
    /// <summary>
    /// Animal represents the animals wandering around the scene
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    internal class Animal : MonoBehaviour, IPoolObject
    {
        internal static Transform Shed { get; set; }

        [SerializeField, Tooltip("Autofilled. Animal Navmesh Agent")]
        private NavMeshAgent agent;
        [SerializeField, Tooltip("Minimum distance before considering agent destination reached")]
        private float stoppingDistance = 0.75f;
        [SerializeField, Tooltip("List of all spawned animals")]
        private AnimalSet spawnedAnimals;

        private bool hiding = false;

#if UNITY_EDITOR
        internal NavMeshAgent Agent => agent;
#endif

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

        /// <summary>
        /// Called from OnWaveEnded Game Event
        /// </summary>
        public void ExitShed()
        {
            hiding = false;
        }

        /// <summary>
        /// Called from OnNightStarted Game Event
        /// </summary>
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

        /// <summary>
        /// Reset object before entering pool
        /// </summary>
        public void OnEnterPool()
        {
            gameObject.SetActive(false);
            hiding = false;
        }

        /// <summary>
        /// Reset object when exiting pool
        /// </summary>
        public void OnExitPool()
        {
            gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        internal void AutofillComponents()
        {
            agent = GetComponent<NavMeshAgent>();
        }
#endif
    }
}
