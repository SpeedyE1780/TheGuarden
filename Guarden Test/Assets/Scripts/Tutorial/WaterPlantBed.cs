using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// WaterPlantBed waits until plant bed is watered
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Water Plant Bed")]
    internal class WaterPlantBed : Tutorial
    {
        private bool watered = false;

        /// <summary>
        /// Called when bucket waters plant bed
        /// </summary>
        public void OnPlantWatered()
        {
            watered = true;
        }

        /// <summary>
        /// Get bucket and listen to OnPlantBedWatered
        /// </summary>
        internal override void Setup()
        {
            watered = false;
        }

        /// <summary>
        /// Wait until plant bed is watered
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            yield return new WaitUntil(() => watered);
        }
    }
}
