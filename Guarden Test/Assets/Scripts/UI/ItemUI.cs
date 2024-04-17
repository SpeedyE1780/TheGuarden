using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Slider progressSlider;

    public void SetItem(string itemName, float progress)
    {
        nameText.text = itemName;
        progressSlider.value = progress;
    }

    public void Select()
    {
        nameText.color = Color.yellow;
    }

    public void Deselect()
    {
        nameText.color = Color.white;
    }
}
