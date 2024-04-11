using System.Collections;
using UnityEngine;

public class PlantTurret : PlantPowerUp
{
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float range;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private SphereCollider turretCollider;

    private Transform targetEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (targetEnemy == null && other.CompareTag("Enemy"))
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
            Instantiate(bullet, shootPoint.position, shootPoint.rotation);
            yield return new WaitForSeconds(cooldown);

            if (Vector3.SqrMagnitude(targetEnemy.position - shootPoint.position) > range)
            {
                targetEnemy = null;
            }
        }
    }

    private void OnValidate()
    {
        turretCollider = GetComponent<SphereCollider>();
        turretCollider.radius = range;
    }
}
