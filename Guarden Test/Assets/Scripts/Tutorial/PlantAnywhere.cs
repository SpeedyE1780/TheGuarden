using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// PlantAnywhere waits until spawned mushroom is fully grown
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Plant Anywhere")]
    internal class PlantAnywhere : ObjectTutorial
    {
        private Mushroom mushroom;
        private bool planted = false;

        /// <summary>
        /// Called when mushroom is planted anywhere
        /// </summary>
        private void OnPlanted()
        {
            planted = true;
        }

        /// <summary>
        /// Get Mushroom and listen to OnPlant
        /// </summary>
        internal override void Setup()
        {
            planted = false;
            mushroom = objectSpawner.SpawnedObject.GetComponent<Mushroom>();
            mushroom.OnPlant.AddListener(OnPlanted);
        }

        /// <summary>
        /// Wait until spawned mushroom is fully grown
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            yield return new WaitUntil(() => planted);
            mushroom.OnPlant.RemoveListener(OnPlanted);
        }
    }
}
