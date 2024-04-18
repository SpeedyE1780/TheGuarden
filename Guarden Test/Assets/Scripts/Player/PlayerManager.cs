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

        /// <summary>
        /// OnPlayerJoin is called from the PlayerInputManager component
        /// </summary>
        /// <param name="player">Player who joined the game</param>
        public void OnPlayerJoin(PlayerInput player)
        {
            player.camera = followCamera.Camera;
            player.uiInputModule = inputSystemUIInputModule;
            player.GetComponent<PlayerInventory>().SetInventoryUI(inventoryUI[player.playerIndex]);
            followCamera.AddTarget(player.transform);
        }

        /// <summary>
        /// OnPlayerLeave is called from the PlayerInputManager component
        /// </summary>
        /// <param name="player">Player who left the game</param>
        public void OnPlayerLeave(PlayerInput player)
        {
            followCamera.RemoveTarget(player.transform);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            inputSystemUIInputModule = FindObjectOfType<InputSystemUIInputModule>();

            if (inputSystemUIInputModule == null)
            {
                GameLogger.LogError("No Input Module in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
