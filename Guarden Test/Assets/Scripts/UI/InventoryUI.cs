using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ItemUI itemPrefab;
    [SerializeField]
    private Transform itemParents;
    [SerializeField]
    private TMP_Text selectedItem;


    public Inventory PlayerInventory { get; set; }

    public void HideSelected()
    {
        selectedItem.gameObject.SetActive(false);
    }

    public void FillUI(List<Mushroom> mushrooms)
    {
        for (int i = itemParents.childCount - 1; i >= 0; i--)
        {
            Destroy(itemParents.GetChild(i).gameObject);
        }

        for (int i = 0; i < mushrooms.Count; i++)
        {
            ItemUI itemUI = Instantiate(itemPrefab, itemParents);
            int index = i;
            itemUI.SetItem(mushrooms[i].name, mushrooms[i].GrowthPercentage, () =>
            {
                PlayerInventory.SelectedItem = index;
                Debug.Log("Selected: " + index);
                gameObject.SetActive(false);
                selectedItem.gameObject.SetActive(true);
                selectedItem.text = mushrooms[index].name;
            });
        }
    }
}
