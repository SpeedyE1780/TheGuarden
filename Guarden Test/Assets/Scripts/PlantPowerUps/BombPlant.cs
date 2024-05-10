using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// BombPlant explodes when enemy enters trigger
    /// </summary>
    internal class BombPlant : PlantPowerUp
    {
        [SerializeField, Tooltip("Delay before explosion")]
        private float delay = 0.0f;
        [SerializeField, Tooltip("Mushrooms health component")]
        private Health mushroomHealth;
        [SerializeField, Tooltip("Mushroom explosion pool")]
        ObjectPool<VFXExplosionController> explosionPool;

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

        /// <summary>
        /// Get all enemy in range and kill them
        /// </summary>
        private void ActivateKill()
        {
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position, Range, AffectedLayer);

            foreach (Collider enemyCollider in enemyColliders)
            {
                GameLogger.LogInfo($"Insta kill destroying {enemyCollider.name}", this, GameLogger.LogCategory.PlantPowerUp);

                if (enemyCollider.TryGetComponent(out Health enemyHealth))
                {
                    enemyHealth.Kill();
                }
            }

            mushroomHealth.Kill();
            explosionPool.GetPooledObject().transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }
}
