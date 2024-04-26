using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// WaterPlantBed waits until plant bed is watered
    /// </summary>
    [CreateAssetMenu(menuName ="Scriptable Objects/Tutorials/Water Plant Bed")]
    internal class WaterPlantBed : ObjectTutorial
    {
        /// <summary>
        /// Wait until plant bed is watered
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            Bucket bucket = objectSpawner.SpawnedObject.GetComponent<Bucket>();
            bool watered = false;
            UnityAction onPlantBedWatered = () => watered = true;
            bucket.OnPlantBedWatered.AddListener(onPlantBedWatered);
            yield return new WaitUntil(() => watered);
            bucket.OnPlantBedWatered.RemoveListener(onPlantBedWatered);
        }
    }
}
