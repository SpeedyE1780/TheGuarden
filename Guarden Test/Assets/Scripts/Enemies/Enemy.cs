using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TheGuarden.NPC;

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
        private float patrolSpeed = 5.0f;
        [SerializeField, Tooltip("Speed at which enemy chases animals")]
        private float chaseSpeed = 7.0f;
        [SerializeField, Tooltip("Speed at which animal is picked up")]
        private float pickUpSpeed = 2.0f;
        [SerializeField, Tooltip("Minimum distance before destination is considered reached")]
        private float distanceThreshold = 3.0f;
        [SerializeField, Tooltip("Radius used to look for animals")]
        private float detectionRadius = 10.0f;
        [SerializeField, Tooltip("Layer used for animal detection")]
        private LayerMask animalMask;
        [SerializeField, Tooltip("Position where animal will be positioned when kidnapped")]
        private Transform holdingPoint;

        private EnemyPath path;
        private Animal targetAnimal;

        private bool ReachedDestination => !agent.pathPending && agent.remainingDistance <= distanceThreshold;
        //When animals are in a force field the path ends at the nearest possible point on the edge of the force field
        //Prevent enemy from holding animal if it can't reach it and it reached the end of the path
        private bool CanGrabAnimal => targetAnimal != null && Vector3.Distance(transform.position, targetAnimal.transform.position) <= distanceThreshold;

#if UNITY_EDITOR
        public float DetectionRadius => detectionRadius;
        public Vector3 TargetPosition => targetAnimal != null ? targetAnimal.transform.position : transform.position;
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
        /// Checks that animal is not null / already kidnapped by other enemy / inside a force field
        /// </summary>
        /// <param name="target">Animal that enemy is trying to kidnap</param>
        /// <returns></returns>
        private bool CanKidnapAnimal(Animal target)
        {
            return target != null && target.Collider.enabled && !target.InsideForceField;
        }

        /// <summary>
        /// Check if animal is within detection range and outside force field
        /// </summary>
        /// <returns>An animal that can be kidnapped or null</returns>
        private Animal DetectAnimal()
        {
            Collider[] animals = Physics.OverlapSphere(transform.position, detectionRadius, animalMask);

            foreach (Collider animalCollider in animals)
            {
                Animal animal = animalCollider.GetComponent<Animal>();

                if (CanKidnapAnimal(animal))
                {
                    return animal;
                }
            }

            return null;
        }

        /// <summary>
        /// Patrol State
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Start Chase State
        /// </summary>
        /// <returns></returns>
        private IEnumerator Chase()
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(targetAnimal.transform.position);

            while (CanKidnapAnimal(targetAnimal))
            {

                if (CanGrabAnimal)
                {
                    StartCoroutine(PickUpTarget());
                    yield break;
                }

                agent.SetDestination(targetAnimal.transform.position);
                yield return null;
            }

            StartCoroutine(Patrol());
        }

        /// <summary>
        /// Start Picking up and holding animal state
        /// </summary>
        /// <returns></returns>
        private IEnumerator PickUpTarget()
        {
            targetAnimal.PauseBehavior();
            agent.SetDestination(transform.position);

            while (targetAnimal.transform.position != holdingPoint.position)
            {
                targetAnimal.transform.position = Vector3.MoveTowards(targetAnimal.transform.position, holdingPoint.position, pickUpSpeed * Time.deltaTime);
                yield return null;
            }

            StartCoroutine(Escape());
        }

        /// <summary>
        /// Start Escape State
        /// </summary>
        /// <returns></returns>
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
    }
}
