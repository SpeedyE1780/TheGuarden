using UnityEngine;
using UnityEngine.InputSystem;

namespace TheGuarden.Players
{
    internal class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("Mesh that will change color base on player id")]
        private MeshRenderer playerRenderer;

        /// <summary>
        /// Called from PlayerInput component
        /// </summary>
        public void OnDropOut(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Update player body color
        /// </summary>
        /// <param name="color">The new player color</param>
        internal void SetColor(Color color)
        {
            playerRenderer.material.color = color;
        }
    }
}
