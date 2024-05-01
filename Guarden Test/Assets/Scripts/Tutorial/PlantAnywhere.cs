using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// PlantAnywhere waits until spawned mushroom is fully grown
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Plant Anywhere")]
    internal class PlantAnywhere : Tutorial
    {
        private bool planted = false;

        /// <summary>
        /// Called when mushroom is planted anywhere
        /// </summary>
        public void OnPlanted()
        {
            planted = true;
        }

        /// <summary>
        /// Get Mushroom and listen to OnPlant
        /// </summary>
        internal override void Setup()
        {
            planted = false;
        }

        /// <summary>
        /// Wait until spawned mushroom is fully grown
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            yield return new WaitUntil(() => planted);
        }
    }
}
