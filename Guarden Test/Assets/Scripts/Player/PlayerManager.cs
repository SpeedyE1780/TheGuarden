using System.Collections;
using System.Collections.Generic;
using TheGuarden.Interactable;
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
        [SerializeField, Tooltip("Items added to player inventory on spawn")]
        private List<GameObject> spawnPlayerItems;
        [SerializeField, Tooltip("List of colors for each player")]
        private List<Color> playerColors;
        [SerializeField, Tooltip("State toggle indicating if player are in scene")]
        private StateToggle playersInScene;
        [SerializeField, Tooltip("Reference of current player control scheme")]
        private StringReference playerControlScheme;

        private List<IPickUp> spawnPickups;

        private void Start()
        {
            spawnPickups = new List<IPickUp>();

            foreach (GameObject item in spawnPlayerItems)
            {
                if (item.TryGetComponent(out IPickUp pickUp))
                {
                    spawnPickups.Add(pickUp);
                }
                else
                {
                    GameLogger.LogError($"{item.name} does not have an IPickUp component", this, GameLogger.LogCategory.Player);
                }
            }
        }

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
            player.camera = followCamera.FollowCamera;
            PlayerController controller = player.GetComponent<PlayerController>();
            int playerIndex = player.playerIndex == -1 ? 0 : player.playerIndex;
            controller.SetColor(playerColors[playerIndex]);
            controller.Inventory.Initialize(spawnPickups, inventoryUI[playerIndex]);
            followCamera.AddTarget(player.transform);
            playersInScene.SetValue(true);
            playerControlScheme.SetValue(PlayerInput.all[0].currentControlScheme);
        }

        /// <summary>
        /// OnPlayerLeave is called from the PlayerInputManager component
        /// </summary>
        /// <param name="player">Player who left the game</param>
        public void OnPlayerLeave(PlayerInput player)
        {
            if (PlayerInput.all.Count == 0)
            {
                playersInScene.SetValue(false);
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
                playerControlScheme.SetValue(PlayerInput.all[0].currentControlScheme);
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
