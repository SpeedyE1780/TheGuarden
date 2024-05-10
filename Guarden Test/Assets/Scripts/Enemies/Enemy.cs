using System.Collections;
using TheGuarden.PlantPowerUps;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// Enemy will follow its path until it reaches the animal shed
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(Health))]
    internal class Enemy : MonoBehaviour, IBehavior, IBuff, IPoolObject
    {
        [SerializeField, Tooltip("Autofilled. Enemy NavMeshAgent component")]
        private NavMeshAgent agent;
        [SerializeField, Tooltip("Minimum distance before destination is considered reached")]
        private float distanceThreshold = 0.5f;
        [SerializeField, Tooltip("Autofilled. Enemy Health component")]
        private Health health;
        [SerializeField, Tooltip("List containing all spawned enemies")]
        private EnemySet enemySet;
        [SerializeField, Tooltip("Game Event raised when enemy reaches the shed")]
        private GameEvent onReachShed;
        [SerializeField, Tooltip("Game Event raised when enemy is killed")]
        private GameEvent onEnemyKilled;
        [SerializeField, Tooltip("Pool that enemy will return to once killed")]
        private ObjectPool<Enemy> enemyPool;

        private EnemyPath path;
        private bool rewinding = false;
        private bool rewindComplete = false;

        private bool ReachedDestination => !agent.pathPending && agent.remainingDistance <= distanceThreshold;
        public NavMeshAgent Agent => agent;
        public Health Health => health;
        public IBuff.OnIBuffDestroy OnIBuffDetroyed { get; set; }

#if UNITY_EDITOR
        internal bool Rewinding => rewinding;
#endif

        void Start()
        {
            health.OnOutOfHealth = () =>
            {
                onEnemyKilled.Raise();
                enemyPool.AddObject(this);
            };
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
        /// Set enemy path
        /// </summary>
        /// <param name="followPath">Path that the enemy will follow</param>
        internal void SetPath(EnemyPath followPath)
        {
            path = followPath;
            StartCoroutine(FollowPath());
        }

        /// <summary>
        /// Set destination to next waypoint until shed is reached
        /// </summary>
        /// <returns></returns>
        private IEnumerator FollowPath()
        {
            agent.SetDestination(path.CurrentPosition);

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
                        onReachShed.Raise();
                        enemyPool.AddObject(this);
                        yield break;
                    }

                    agent.SetDestination(path.CurrentPosition);
                }

                yield return null;
            }
        }

        /// <summary>
        /// Make enemy rewind its path
        /// </summary>
        /// <param name="waypoints">Number of waypoints enemy will rewind</param>
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

        /// <summary>
        /// Set destination to previous waypoint until rewind is complete or reached start of path
        /// </summary>
        /// <param name="waypoints">Number of waypoints enemy will rewind</param>
        /// <returns></returns>
        private IEnumerator RewindPath(int waypoints)
        {
            rewinding = true;
            path.CurrentIndex -= 1;
            agent.SetDestination(path.CurrentPosition);

            while (waypoints > 0)
            {
                yield return new WaitUntil(() => ReachedDestination);

                GameLogger.LogInfo($"{name} rewinded stops remaining: {waypoints}", this, GameLogger.LogCategory.Enemy);
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

            GameLogger.LogInfo($"{name} completed rewind", this, GameLogger.LogCategory.Enemy);
            rewindComplete = true;
            rewinding = false;

            StartCoroutine(FollowPath());
        }

        /// <summary>
        /// Reset state to default before entering pool
        /// </summary>
        public void OnEnterPool()
        {
            gameObject.SetActive(false);
            health.ResetHealth();
            rewindComplete = false;
        }

        /// <summary>
        /// Enable behaviors when exiting pools
        /// </summary>
        public void OnExitPool()
        {
            gameObject.SetActive(true);
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
