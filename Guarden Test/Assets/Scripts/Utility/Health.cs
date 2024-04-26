using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Health represents an objects health and destroys it once it reaches zero
    /// </summary>
    public class Health : MonoBehaviour
    {
        [SerializeField, Tooltip("Maximum Health")]
        private int maxHealth;

        private int health;

        private void Start()
        {
            health = maxHealth;
        }

        /// <summary>
        /// Damage player and destroy if health <= 0
        /// </summary>
        /// <param name="damage">Damage received</param>
        public void Damage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Heal player
        /// </summary>
        /// <param name="heal">Health received</param>
        public void Heal(int heal)
        {
            health = Mathf.Clamp(health + heal, 0, maxHealth);
        }
    }
}
