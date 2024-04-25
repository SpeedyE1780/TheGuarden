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

        public void Damage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void Heal(int heal)
        {
            health = Mathf.Clamp(health + heal, 0, maxHealth);
        }
    }
}
