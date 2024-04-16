using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private Button button;

    public void SetItem(string itemName, float progress, bool select, UnityAction buttonCallback)
    {
        nameText.text = itemName;
        progressSlider.value = progress;
        button.onClick.AddListener(buttonCallback);

        if(select)
        {
            button.Select();
        }
    }
}
