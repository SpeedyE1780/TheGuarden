using System.Collections;
using UnityEngine;
using TheGuarden.Interactable;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/Plant In Soil")]
    internal class PlantInSoil : ObjectTutorial
    {
        internal override IEnumerator StartTutorial()
        {
            Mushroom mushroom = objectSpawner.SpawnedObject.GetComponent<Mushroom>();
            bool planted = false;
            UnityAction OnPlantInSoil = () => planted = true;
            mushroom.OnPlantInSoil.AddListener(OnPlantInSoil);
            yield return new WaitUntil(() => planted);
            mushroom.OnPlantInSoil.RemoveListener(OnPlantInSoil);
        }
    }
}
