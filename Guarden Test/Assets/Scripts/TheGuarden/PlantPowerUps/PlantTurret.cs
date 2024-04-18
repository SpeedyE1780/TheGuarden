using System.Collections;
using UnityEngine;
using TheGuarden.Utility;

namespace TheGuarden.PlantPowerUps
{
    public class PlantTurret : PlantPowerUp
    {
        [SerializeField]
        private Transform shootPoint;
        [SerializeField]
        private Projectile projectilePrefab;
        [SerializeField]
        private float cooldown;

        private Transform targetEnemy;

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
