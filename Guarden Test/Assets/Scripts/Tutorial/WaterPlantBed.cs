using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName ="Scriptable Objects/Tutorial/Water Plant Bed")]
    internal class WaterPlantBed : ObjectTutorial
    {
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
