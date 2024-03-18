using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemPrefab;
    [SerializeField]
    private Transform itemParents;


    public Inventory PlayerInventory { get; set; }

    public void FillUI(List<GrowPlant> plants)
    {
        for (int i = itemParents.childCount - 1; i >= 0; i--)
        {
            Destroy(itemParents.GetChild(i).gameObject);
        }

        for (int i = 0; i < plants.Count; i++)
        {
            ItemUI itemUI = Instantiate(itemPrefab, itemParents);
            int index = i;
            itemUI.SetItem(plants[i].name, plants[i].GrowthPercentage, () =>
            {
                PlayerInventory.SelectedItem = index;
                Debug.Log("Selected: " + index);
                gameObject.SetActive(false);
            });
        }
    }
}
