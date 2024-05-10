using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// PlantInSoil waits until mushroom is planted in soil
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Plant In Soil")]
    internal class PlantInSoil : Tutorial
    {
        private bool planted = false;

        /// <summary>
        /// Called when mushroom planted in soil
        /// </summary>
        public void OnPlantedInSoil()
        {
            planted = true;
        }

        /// <summary>
        /// Get mushroom and listen to OnPlantInSoil
        /// </summary>
        internal override void Setup()
        {
            planted = false;
        }

        /// <summary>
        /// Wait until mushroom is planted in soil
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            yield return new WaitUntil(() => planted);
        }
    }
}
