using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private FollowTarget followCamera;
    [SerializeField]
    private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField]
    private List<InventoryUI> inventoryUI;

    public void OnPlayerJoin(PlayerInput player)
    {
        player.camera = followCamera.Camera;
        player.uiInputModule = inputSystemUIInputModule;
        player.GetComponent<Inventory>().SetInventoryUI(inventoryUI[player.playerIndex]);
        followCamera.AddTarget(player.transform);
    }

    public void OnPlayerLeave(PlayerInput player)
    {
        followCamera.RemoveTarget(player.transform);
    }
}
