using System.Collections;
using TheGuarden.PlantPowerUps;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// Enemy is a State Machine that will patrol the scene and try to kidnap animals
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(Health))]
    public class Enemy : MonoBehaviour, IBehavior, IBuff
    {
        [SerializeField, Tooltip("Autofilled. Enemy NavMeshAgent component")]
        private NavMeshAgent agent;
        [SerializeField, Tooltip("Speed at which enemy patrol the scene")]
        private float agentSpeed = 5.0f;
        [SerializeField, Tooltip("Minimum distance before destination is considered reached")]
        private float distanceThreshold = 3.0f;
        [SerializeField, Tooltip("Autofilled. Enemy Health component")]
        private Health health;
        [SerializeField, Tooltip("List containing all spawned enemies")]
        private EnemySet enemySet;

        private EnemyPath path;
        private bool rewinding = false;
        private bool rewindComplete = false;

        internal UnityAction<GameObject> OnDestroyed { get; set; }

        private bool ReachedDestination => !agent.pathPending && agent.remainingDistance <= distanceThreshold;
        public NavMeshAgent Agent => agent;
        public Health Health => health;
        public IBuff.OnIBuffDestroy OnIBuffDetroyed { get; set; }

#if UNITY_EDITOR
        internal bool Rewinding => rewinding;
#endif

        void Start()
        {
            StartCoroutine(Patrol());
        }

        private void OnEnable()
        {
            enemySet.Add(this);
        }

        private void OnDisable()
        {
            enemySet.Remove(this);
            OnIBuffDetroyed?.Invoke(this);
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
        /// Patrol State
        /// </summary>
        /// <returns></returns>
        private IEnumerator Patrol()
        {
            agent.SetDestination(path.CurrentPosition);
            agent.speed = agentSpeed;

            while (true)
            {
                if (rewinding)
                {
                    yield break;
                }

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

                yield return null;
            }
        }

        private void OnDestroy()
        {
            OnDestroyed(gameObject);
        }

        public void RewindPathProgress(int waypoints)
        {
            if (rewindComplete)
            {
                GameLogger.LogInfo($"{name} already rewinded path", this, GameLogger.LogCategory.Enemy);
                return;
            }

            GameLogger.LogInfo($"{name} rewinded by {waypoints} waypoints", this, GameLogger.LogCategory.Enemy);
            StartCoroutine(RewindPath(waypoints));
        }

        private IEnumerator RewindPath(int waypoints)
        {
            rewinding = true;
            path.CurrentIndex -= 1;
            agent.SetDestination(path.CurrentPosition);

            while (waypoints > 0)
            {
                if (ReachedDestination)
                {
                    GameLogger.LogInfo($"{name} rewinded stop remaining {waypoints}", this, GameLogger.LogCategory.Enemy);
                    path.CurrentIndex -= 1;
                    waypoints -= 1;

                    if (path.CurrentIndex < 0)
                    {
                        path.CurrentIndex = 0;
                        GameLogger.LogInfo("Early break path index is less than 0", this, GameLogger.LogCategory.Enemy);
                        break;
                    }

                    agent.SetDestination(path.CurrentPosition);
                }

                yield return null;
            }

            GameLogger.LogInfo($"{name} completed rewind", this, GameLogger.LogCategory.Enemy);
            yield return new WaitUntil(() => ReachedDestination);
            rewindComplete = true;
            rewinding = false;

            StartCoroutine(Patrol());
        }

#if UNITY_EDITOR
        internal void AutofillComponents()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
#endif
    }
}
