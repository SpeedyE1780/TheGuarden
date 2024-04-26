using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// PlantAnywhere waits until spawned mushroom is fully grown
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Plant Anywhere")]
    internal class PlantAnywhere : ObjectTutorial
    {
        /// <summary>
        /// Wait until spawned mushroom is fully grown
        /// </summary>
        /// <returns></returns>
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
