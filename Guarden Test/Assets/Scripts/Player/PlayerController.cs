using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheGuarden.Players
{
    /// <summary>
    /// PlayerController handles main player behaviors
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    internal class PlayerController : MonoBehaviour
    {
        private const string PlayerActionsMap = "PlayerActions";
        private const string PopupWindowMap = "PopupWindow";

        [SerializeField, Tooltip("Mesh that will change color base on player id")]
        private MeshRenderer playerRenderer;
        [SerializeField, Tooltip("Player's inventory")]
        private PlayerInventory inventory;
        [SerializeField, Tooltip("Player's input component")]
        private PlayerInput input;
        [SerializeField, Tooltip("Game event raised to hide active window")]
        private GameEvent hideActiveWindow;
        [SerializeField, Tooltip("Game event raised to start enemy wave")]
        private GameEvent startEnemyWave;

        public PlayerInventory Inventory => inventory;

        /// <summary>
        /// Called from PlayerInput component
        /// </summary>
        public void OnDropOut(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                inventory.EmptyInventory();
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Called from game event switch action map to hide window
        /// </summary>
        public void OnWindowActive()
        {
            input.SwitchCurrentActionMap(PopupWindowMap);
        }

        /// <summary>
        /// Hide active window when input is pressed
        /// </summary>
        /// <param name="context"></param>
        public void HideActiveWindow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                input.SwitchCurrentActionMap(PlayerActionsMap);
                hideActiveWindow.Raise();
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

        /// <summary>
        /// Start enemy wave
        /// </summary>
        /// <param name="context">context is used to check if input is performed</param>
        public void StartEnemyWave(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                startEnemyWave.Raise();
            }
        }
    }
}
