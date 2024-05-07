using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TheGuarden.UI;
using TheGuarden.Utility;

namespace TheGuarden.Players
{
    /// <summary>
    /// PlayerManager handles players joining and leaving games
    /// </summary>
    [RequireComponent(typeof(PlayerInputManager))]
    internal class PlayerManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Camera following players")]
        private FollowTarget followCamera;
        [SerializeField, Tooltip("Autofilled. Input module used for UI")]
        private InputSystemUIInputModule inputSystemUIInputModule;
        [SerializeField, Tooltip("List of each player's inventory UI")]
        private List<InventoryUI> inventoryUI;
        [SerializeField, Tooltip("List of colors for each player")]
        private List<Color> playerColors;

        /// <summary>
        /// OnPlayerJoin is called from the PlayerInputManager component
        /// </summary>
        /// <param name="player">Player who joined the game</param>
        public void OnPlayerJoin(PlayerInput player)
        {
            player.camera = followCamera.Camera;
            PlayerController controller = player.GetComponent<PlayerController>();
            controller.SetColor(playerColors[player.playerIndex]);
            controller.Inventory.SetInventoryUI(inventoryUI[player.playerIndex]);
            followCamera.AddTarget(player.transform);
        }

        /// <summary>
        /// OnPlayerLeave is called from the PlayerInputManager component
        /// </summary>
        /// <param name="player">Player who left the game</param>
        public void OnPlayerLeave(PlayerInput player)
        {
            if (followCamera != null)
            {
                followCamera.RemoveTarget(player.transform);
            }
        }

#if UNITY_EDITOR
        internal void AutofillVariables()
        {
            followCamera = FindObjectOfType<FollowTarget>();

            if (followCamera == null)
            {
                GameLogger.LogError("No FollowTarget in scene", gameObject, GameLogger.LogCategory.Scene);
            }

            inputSystemUIInputModule = FindObjectOfType<InputSystemUIInputModule>();

            if (inputSystemUIInputModule == null)
            {
                GameLogger.LogError("No Input Module in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
