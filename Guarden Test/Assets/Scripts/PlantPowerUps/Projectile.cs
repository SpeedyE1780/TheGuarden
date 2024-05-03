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
        [SerializeField]
        private ObjectPool<Projectile> pool;

        public Transform Target { get; set; }

        private void Update()
        {
            if (Target == null)
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
            Health targetHealth = Target.GetComponent<Health>();

            if (targetHealth == null)
            {
                GameLogger.LogError($"{Target.name} has no health component and can't be damaged", this, GameLogger.LogCategory.PlantPowerUp);
                return;
            }

            targetHealth.Damage(damage);
        }

        public void OnEnterPool()
        {
            gameObject.SetActive(false);
            Target = null;
        }

        public void OnExitPool()
        {
            gameObject.SetActive(true);
        }
    }
}
