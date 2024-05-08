using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheGuarden.Players
{
    internal class PlayerController : MonoBehaviour
    {
        private const string PlayerActionsMap = "PlayerActions";
        private const string PopupWindowMap = "PopupWindow";

        [SerializeField, Tooltip("Mesh that will change color base on player id")]
        private MeshRenderer playerRenderer;
        [SerializeField]
        private PlayerInventory inventory;
        [SerializeField]
        private PlayerInput input;
        [SerializeField]
        private GameEvent hideMushroomUnlockedWindow;

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

        public void OnMushroomUnlocked()
        {
            input.SwitchCurrentActionMap(PopupWindowMap);
        }

        public void OnHideMushroomUnlockedWindow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                input.SwitchCurrentActionMap(PlayerActionsMap);
                hideMushroomUnlockedWindow.Raise();
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
