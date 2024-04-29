using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// WaterPlantBed waits until plant bed is watered
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Water Plant Bed")]
    internal class WaterPlantBed : ObjectTutorial
    {
        private Bucket bucket;
        private bool watered = false;

        /// <summary>
        /// Called when bucket waters plant bed
        /// </summary>
        private void OnPlantWatered()
        {
            watered = true;
        }

        /// <summary>
        /// Get bucket and listen to OnPlantBedWatered
        /// </summary>
        internal override void Setup()
        {
            watered = false;
            bucket = objectSpawner.SpawnedObject.GetComponent<Bucket>();
            bucket.OnPlantBedWatered.AddListener(OnPlantWatered);
        }

        /// <summary>
        /// Wait until plant bed is watered
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            yield return new WaitUntil(() => watered);
            bucket.OnPlantBedWatered.RemoveListener(OnPlantWatered);
        }
    }
}
