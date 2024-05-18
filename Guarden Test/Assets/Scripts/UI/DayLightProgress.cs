using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace TheGuarden.UI
{
    /// <summary>
    /// DayLightProgress updates a slider to show when night will start
    /// </summary>
    public class DayLightProgress : MonoBehaviour
    {
        [SerializeField, Tooltip("Slider showing day progress")]
        private Slider dayProgress;
        [SerializeField, Tooltip("DayLight cycle in scene")]
        private DayLightCycle cycle;

        private void Update()
        {
            dayProgress.value = cycle.DayProgess;
        }
    }
}
