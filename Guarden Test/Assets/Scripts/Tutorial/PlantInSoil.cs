using System.Collections;
using UnityEngine;
using TheGuarden.Interactable;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// PlantInSoil waits until mushroom is planted in soil
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Plant In Soil")]
    internal class PlantInSoil : ObjectTutorial
    {
        /// <summary>
        /// Wait until mushroom is planted in soil
        /// </summary>
        /// <returns></returns>
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
