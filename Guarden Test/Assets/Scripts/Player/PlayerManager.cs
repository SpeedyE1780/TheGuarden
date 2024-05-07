using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TheGuarden.UI;
using TheGuarden.Utility;
using UnityEngine.InputSystem.Users;
using System.Collections;

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

        private void OnEnable()
        {
            InputUser.onUnpairedDeviceUsed += OnUnpairDeviceUsed;

        }

        private void OnDisable()
        {
            InputUser.onUnpairedDeviceUsed -= OnUnpairDeviceUsed;
        }

        /// <summary>
        /// OnPlayerJoin is called from the PlayerInputManager component
        /// </summary>
        /// <param name="player">Player who joined the game</param>
        public void OnPlayerJoin(PlayerInput player)
        {
            player.camera = followCamera.Camera;
            PlayerController controller = player.GetComponent<PlayerController>();
            int playerIndex = player.playerIndex == -1 ? 0 : player.playerIndex;
            controller.SetColor(playerColors[playerIndex]);
            controller.Inventory.SetInventoryUI(inventoryUI[playerIndex]);
            followCamera.AddTarget(player.transform);
        }

        private void OnUnpairDeviceUsed(InputControl arg1, UnityEngine.InputSystem.LowLevel.InputEventPtr arg2)
        {
            StartCoroutine(SwitchControlScheme(arg1.device));
        }

        private IEnumerator SwitchControlScheme(InputDevice device)
        {
            //Wait one frame in case this call triggers a player join in the player input manager
            yield return null;

            if (PlayerInput.FindFirstPairedToDevice(device) != null)
            {
                GameLogger.LogWarning($"Device {device.name} already paired", this, GameLogger.LogCategory.Player);
                yield break;
            }

            if (PlayerInput.all.Count == 0)
            {
                GameLogger.LogWarning($"No player available to pair with {device.name}", this, GameLogger.LogCategory.Player);
                yield break;
            }

            bool success = PlayerInput.all[0].SwitchCurrentControlScheme(device);

            if (success)
            {
                GameLogger.LogInfo($"Switched player 1 control scheme to {device.name}", this, GameLogger.LogCategory.Player);
            }
            else
            {
                GameLogger.LogError($"Unable to switch player 1 control scheme to {device.name}", this, GameLogger.LogCategory.Player);
            }
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
