using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Transform growthImage;
    [SerializeField]
    private Button button;

    public void SetItem(string itemName, float growthPercent, UnityAction buttonCallback)
    {
        nameText.text = itemName;
        growthImage.localScale = new Vector3 (1, growthPercent, 1);
        button.onClick.AddListener(buttonCallback);
    }
}
