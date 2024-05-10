using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Projectile that follows targets and destroys it
    /// </summary>
    internal class Projectile : MonoBehaviour, IPoolObject
    {
        [SerializeField, Tooltip("Projectile speed")]
        private float speed;
        [SerializeField, Tooltip("Damage dealt to target")]
        private float damage = 1.0f;
        [SerializeField, Tooltip("Pool projectile should return to")]
        private ObjectPool<Projectile> pool;

        internal Transform Target { get; set; }

        private void Update()
        {
            if (!Target.gameObject.activeSelf)
            {
                pool.AddObject(this);
                return;
            }

            transform.forward = Target.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (transform.position == Target.position)
            {
                DamageTarget();
                pool.AddObject(this);
            }
        }

        private void DamageTarget()
        {
            if (!Target.TryGetComponent(out Health targetHealth))
            {
                GameLogger.LogError($"{Target.name} has no health component and can't be damaged", this, GameLogger.LogCategory.PlantPowerUp);
                return;
            }

            targetHealth.Damage(damage);
        }

        /// <summary>
        /// Reset state before entering pool
        /// </summary>
        public void OnEnterPool()
        {
            gameObject.SetActive(false);
            Target = null;
        }

        /// <summary>
        /// Reset to initial state befor exiting pool
        /// </summary>
        public void OnExitPool()
        {
            gameObject.SetActive(true);
        }
    }
}
