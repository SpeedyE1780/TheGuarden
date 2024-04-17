using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemPrefab;
    [SerializeField]
    private Transform itemParents;

    private List<ItemUI> items = new List<ItemUI>();

    public void AddItem(IInteractable item)
    {
        ItemUI itemUI = Instantiate(itemPrefab, itemParents);
        itemUI.SetItem(item.Name, item.UsabilityPercentage);
        items.Add(itemUI);
    }

    public void SelectItem(int index)
    {
        items[index].Select();
    }

    public void DeselectItem(int index)
    {
        items[index].Deselect();
    }

    public void RemoveItem(int index)
    {
        Destroy(items[index].gameObject);
        items.RemoveAt(index);
    }
}
