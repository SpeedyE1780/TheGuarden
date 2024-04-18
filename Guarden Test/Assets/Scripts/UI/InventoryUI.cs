using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemPrefab;
    [SerializeField]
    private Transform itemParents;

    private List<ItemUI> items = new List<ItemUI>();

    public ItemUI AddItem(IInventoryItem item)
    {
        ItemUI itemUI = Instantiate(itemPrefab, itemParents);
        itemUI.SetItem(this, item.Name, item.UsabilityPercentage);
        items.Add(itemUI);
        return itemUI;
    }

    public void SelectItem(int index)
    {
        items[index].Select();
    }

    public void DeselectItem(int index)
    {
        items[index].Deselect();
    }

    public void RemoveItem(ItemUI item)
    {
        items.Remove(item);
    }
}
