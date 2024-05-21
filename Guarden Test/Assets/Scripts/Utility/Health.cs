using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Health represents an objects health and destroys it once it reaches zero
    /// </summary>
    public class Health : MonoBehaviour
    {
        public delegate void OutOfHealth();

        [SerializeField, Tooltip("Maximum Health")]
        private float maxHealth;

        public float CurrentMaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public OutOfHealth OnOutOfHealth { get; set; }

        private void Start()
        {
            ResetHealth();
        }

        /// <summary>
        /// Damage player and destroy if health <= 0
        /// </summary>
        /// <param name="damage">Damage received</param>
        public void Damage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0.0f)
            {
                OnOutOfHealth();
            }
        }

        /// <summary>
        /// Heal player
        /// </summary>
        /// <param name="heal">Health received</param>
        public void Heal(float heal)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + heal, 0.0f, CurrentMaxHealth);
        }

        /// <summary>
        /// Reset max health and current health
        /// </summary>
        public void ResetHealth()
        {
            CurrentMaxHealth = maxHealth;
            CurrentHealth = CurrentMaxHealth;
        }

        /// <summary>
        /// Reduce health down to 0
        /// </summary>
        public void Kill()
        {
            Damage(CurrentHealth);
        }

        /// <summary>
        /// Multiply max health
        /// </summary>
        /// <param name="multiplier">Max health multiplier</param>
        /// <param name="updateHealth">Whether or not current health should be updated to new max health</param>
        public void MutlitplyMaxHealth(float multiplier, bool updateHealth = true)
        {
            CurrentMaxHealth = maxHealth * multiplier;
            GameLogger.LogInfo($"{name} max health now is {CurrentMaxHealth}", this, GameLogger.LogCategory.Enemy | GameLogger.LogCategory.Plant);

            if (updateHealth)
            {
                CurrentHealth = CurrentMaxHealth;
            }
        }
    }
}
