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
        private void OnEnable()
        {
            StartCoroutine(DetectEnemy());
        }

        private IEnumerator DetectEnemy()
        {
            Collider[] enemyCollider = new Collider[1];

            while (true)
            {
                int count = Physics.OverlapSphereNonAlloc(transform.position, Range, enemyCollider, AffectedLayer);

                if (count > 0)
                {
                    targetEnemy = enemyCollider[0].transform;
                    yield return ShootTurret();
                }

                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// Shoot projectiles until enemy is out of range or destroyed
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShootTurret()
        {
            GameLogger.LogInfo($"{name} targeting {targetEnemy.name}", this, GameLogger.LogCategory.PlantPowerUp);

            while (targetEnemy != null && targetEnemy.gameObject.activeSelf)
            {
                Projectile projectile = projectilePool.GetPooledObject();
                audioSource.Play();
                projectile.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
                projectile.Target = targetEnemy;
                yield return new WaitForSeconds(cooldown);

                if (!targetEnemy.gameObject.activeSelf || Vector3.Distance(targetEnemy.position, shootPoint.position) > Range)
                {
                    targetEnemy = null;
                    yield break;
                }
            }
        }
    }
}
