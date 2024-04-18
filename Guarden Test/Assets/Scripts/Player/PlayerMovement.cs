using UnityEngine;
using UnityEngine.InputSystem;

namespace TheGuarden.Players
{
    /// <summary>
    /// PlayerMovement handles player movement
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
    internal class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Tooltip("Movement speed")]
        private float speed = 5f;
        [SerializeField, Tooltip("Autofilled. Player rigidbody")]
        private Rigidbody rb;

        private Vector3 movement;
        private Vector3 velocity;

        /// <summary>
        /// Update movement x z variables on input
        /// </summary>
        /// <param name="context">Input context</param>
        public void OnMovement(InputAction.CallbackContext context)
        {
            movement.x = context.ReadValue<Vector2>().x;
            movement.z = context.ReadValue<Vector2>().y;
        }

        private void Update()
        {
            if (movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            velocity = movement * speed;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            rb = GetComponent<Rigidbody>();
        }
#endif
    }
}
