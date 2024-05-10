using System.Collections;
using System.Collections.Generic;
using TheGuarden.UI;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;

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
        [SerializeField, Tooltip("State toggle indicating if player are in scene")]
        private StateToggle playersInScene;

        private void OnEnable()
        {
            InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
        }

        private void OnDisable()
        {
            InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
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
            playersInScene.TurnOn();
        }

        /// <summary>
        /// OnPlayerLeave is called from the PlayerInputManager component
        /// </summary>
        /// <param name="player">Player who left the game</param>
        public void OnPlayerLeave(PlayerInput player)
        {
            if (PlayerInput.all.Count == 0)
            {
                playersInScene.TurnOff();
            }

            if (followCamera != null)
            {
                followCamera.RemoveTarget(player.transform);
            }
        }

        /// <summary>
        /// Wait until unpaired device is used
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnUnpairedDeviceUsed(InputControl control, UnityEngine.InputSystem.LowLevel.InputEventPtr eventPtr)
        {
            StartCoroutine(SwitchControlScheme(control.device));
        }

        /// <summary>
        /// Try to switch player 1 to the unpaired device
        /// </summary>
        /// <param name="device">Device trying to pair to</param>
        /// <returns></returns>
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
