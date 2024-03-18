using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemPrefab;

    public Inventory PlayerInventory { get; set; }

    public void FillUI(List<GrowPlant> plants)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < plants.Count; i++)
        {
            ItemUI itemUI = Instantiate(itemPrefab, transform);
            itemUI.SetItem(plants[i].name, plants[i].GrowthPercentage, () => { PlayerInventory.SelectedItem = i; });
        }
    }
}
