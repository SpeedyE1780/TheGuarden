using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    public class PlantInstaKill : PlantPowerUp
    {
        [SerializeField]
        private float delay = 0.0f;
        [SerializeField]
        private Health mushroomHealth;

        private void OnTriggerEnter(Collider other)
        {
            if (delay > 0.0f)
            {
                Invoke(nameof(ActivateKill), delay);
            }
            else
            {
                ActivateKill();
            }
        }

        private void ActivateKill()
        {
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position, Range, AffectedLayer);

            foreach (Collider enemyCollider in enemyColliders)
            {
                GameLogger.LogInfo($"Insta kill destroying {enemyCollider.name}", this, GameLogger.LogCategory.PlantPowerUp);

                if (enemyCollider.TryGetComponent<Health>(out Health enemyHealth))
                {
                    enemyHealth.Kill();
                }
            }

            mushroomHealth.Kill();
        }
    }
}
