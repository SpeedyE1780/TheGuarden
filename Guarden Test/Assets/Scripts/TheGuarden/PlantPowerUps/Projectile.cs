using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// Projectile that follows targets and destroys it
    /// </summary>
    internal class Projectile : MonoBehaviour
    {
        [SerializeField, Tooltip("Projectile speed")]
        private float speed;

        public Transform Target { get; set; }

        private void Update()
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.forward = Target.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (transform.position == Target.position)
            {
                Destroy(gameObject);
                Destroy(Target.gameObject);
            }
        }
    }
}
