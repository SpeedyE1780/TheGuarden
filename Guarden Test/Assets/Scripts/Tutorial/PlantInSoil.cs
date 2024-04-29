using System.Collections;
using UnityEngine;
using TheGuarden.Interactable;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// PlantInSoil waits until mushroom is planted in soil
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Plant In Soil")]
    internal class PlantInSoil : ObjectTutorial
    {
        private Mushroom mushroom;
        private bool planted = false;

        /// <summary>
        /// Called when mushroom planted in soil
        /// </summary>
        private void OnPlantedInSoil()
        {
            planted = true;
        }

        /// <summary>
        /// Get mushroom and listen to OnPlantInSoil
        /// </summary>
        internal override void Setup()
        {
            planted = false;
            mushroom = objectSpawner.SpawnedObject.GetComponent<Mushroom>();
            mushroom.OnPlantInSoil.AddListener(OnPlantedInSoil);
        }

        /// <summary>
        /// Wait until mushroom is planted in soil
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            
            yield return new WaitUntil(() => planted);
            mushroom.OnPlantInSoil.RemoveListener(OnPlantedInSoil);
        }
    }
}
