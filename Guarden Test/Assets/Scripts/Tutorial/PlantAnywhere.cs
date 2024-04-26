using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/Plant Anywhere")]
    internal class PlantAnywhere : ObjectTutorial
    {
        internal override IEnumerator StartTutorial()
        {
            Mushroom mushroom = objectSpawner.SpawnedObject.GetComponent<Mushroom>();
            bool planted = false;
            UnityAction onPlantedAnywhere = () => planted = true;
            mushroom.OnPlant.AddListener(onPlantedAnywhere);
            yield return new WaitUntil(() => planted);
            mushroom.OnPlant.RemoveListener(onPlantedAnywhere);
        }
    }
}
