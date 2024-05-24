using System.Collections;
using TheGuarden.Utility;
using UnityEngine;

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
        private ObjectPool<Projectile> projectilePool;
        [SerializeField, Tooltip("Cooldown between projectiles")]
        private float cooldown;
        [SerializeField, Tooltip("Audio Source")]
        private AudioSource audioSource;

        private Transform targetEnemy;

#if UNITY_EDITOR
        public Transform TargetEnemy => targetEnemy;
#endif

        private void OnTriggerStay(Collider other)
        {
            if (targetEnemy == null && other.CompareTag(Tags.Enemy))
            {
                targetEnemy = other.transform;
                StartCoroutine(ShootTurret(targetEnemy));
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
        /// <param name="enemy">Enemy targeted in this coroutine if target enemy is changed during wait instruction this coroutine is stopped</param>
        /// <returns></returns>
        private IEnumerator ShootTurret(Transform enemy)
        {
            GameLogger.LogInfo($"{name} targeting {enemy.name}", this, GameLogger.LogCategory.PlantPowerUp);

            while (enemy != null && enemy == targetEnemy && enemy.gameObject.activeSelf)
            {
                Projectile projectile = projectilePool.GetPooledObject();
                audioSource.Play();
                projectile.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
                projectile.Target = enemy;
                yield return new WaitForSeconds(cooldown);

                if (enemy != null && Vector3.SqrMagnitude(enemy.position - shootPoint.position) > Range)
                {
                    enemy = null;
                }
            }
        }
    }
}
