using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemPrefab;
    [SerializeField]
    private Transform itemParents;
    [SerializeField]
    private TMP_Text selectedItem;
    [SerializeField]
    private EventSystem eventSystem;

    public Inventory PlayerInventory { get; set; }

    public void HideSelected()
    {
        selectedItem.gameObject.SetActive(false);
    }

    public void FillUI(List<IInteractable> items)
    {
        for (int i = itemParents.childCount - 1; i >= 0; i--)
        {
            Destroy(itemParents.GetChild(i).gameObject);
        }

        for (int i = 0; i < items.Count; i++)
        {
            ItemUI itemUI = Instantiate(itemPrefab, itemParents);
            int index = i;

            if (i == 0)
            {
                eventSystem.SetSelectedGameObject(itemUI.gameObject); 
            }

            itemUI.SetItem(items[i].Name, items[i].UsabilityPercentage, i == 0, () =>
            {
                PlayerInventory.SetSelectedItem(index);
                GameLogger.LogInfo("Selected: " + index, gameObject, GameLogger.LogCategory.UI);
                gameObject.SetActive(false);
                selectedItem.gameObject.SetActive(true);
                selectedItem.text = items[index].Name;
            });
        }
    }
}
