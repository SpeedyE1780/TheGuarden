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

        private float health;
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
            health -= damage;

            if (health <= 0.0f)
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
            health = Mathf.Clamp(health + heal, 0.0f, maxHealth);
        }

        public void ResetHealth()
        {
            health = maxHealth;
        }

        public void MutlitplyMaxHealth(float multiplier, bool updateHealth = true)
        {
            maxHealth *= multiplier;
            GameLogger.LogInfo($"{name} max health now is {maxHealth}", this, GameLogger.LogCategory.Enemy | GameLogger.LogCategory.Plant);

            if (updateHealth)
            {
                health = maxHealth;
            }
        }
    }
}
