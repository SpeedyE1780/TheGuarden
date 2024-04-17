using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField]
    private InventoryUI inventoryUI;

    public void OnPlayerJoin(PlayerInput player)
    {
        player.camera = playerCamera;
        player.uiInputModule = inputSystemUIInputModule;
        player.GetComponent<Inventory>().SetInventoryUI(inventoryUI);
    }

    public void OnPlayerLeave(PlayerInput player)
    {

    }
}
