using System.Collections;
using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Plant that can shoot projectiles at enemy in trigger
    /// </summary>
    internal class PlantTurret : PlantPowerUp
    {
        [SerializeField, Tooltip("Point where projectile will spawn")]
        private Transform shootPoint;
        [SerializeField, Tooltip("Projectile prefab")]
        private Projectile projectilePrefab;
        [SerializeField, Tooltip("Cooldown between projectiles")]
        private float cooldown;

        private Transform targetEnemy;

#if UNITY_EDITOR
        public Transform TargetEnemy => targetEnemy;
#endif

        private void OnTriggerStay(Collider other)
        {
            if (targetEnemy == null && other.CompareTag(Tags.Enemy))
            {
                targetEnemy = other.transform;
                StartCoroutine(ShootTurret());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == targetEnemy)
            {
                targetEnemy = null;
            }
        }

        /// <summary>
        /// Shoot projectiles until enemy is out of range or destroyed
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShootTurret()
        {
            while (targetEnemy != null)
            {
                Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                projectile.Target = targetEnemy;
                yield return new WaitForSeconds(cooldown);

                if (targetEnemy != null && Vector3.SqrMagnitude(targetEnemy.position - shootPoint.position) > powerUpRange)
                {
                    targetEnemy = null;
                }
            }
        }
    }
}
