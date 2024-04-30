using TMPro;
using UnityEngine;

namespace TheGuarden.UI
{
    public class AnimalTracker : MonoBehaviour
    {
        [SerializeField, Tooltip("Tracker text")]
        private TextMeshProUGUI text;

        public void UpdateText(int animalCount)
        {
            text.text = $"Animals: {animalCount}";
        }
    }
}
