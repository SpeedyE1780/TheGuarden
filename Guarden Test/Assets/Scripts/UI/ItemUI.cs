using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField, Tooltip("Item name text")]
        private TMP_Text nameText;
        [SerializeField, Tooltip("Item progress slider")]
        private Slider progressSlider;

        private InventoryUI inventoryUI;

        public void SetParent(InventoryUI inventory)
        {
            inventoryUI = inventory;
        }

        public void SetItem(string itemName, float progress)
        {
            nameText.text = itemName;
            progressSlider.value = progress;
        }

        public void SetProgress(float progress)
        {
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

        private void OnDestroy()
        {
            if (inventoryUI != null)
            {
                inventoryUI.RemoveItem(this);
            }
        }
    }
}
