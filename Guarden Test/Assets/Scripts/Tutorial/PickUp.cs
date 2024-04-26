using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// PickUp waits until spawned object is picked up
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Pick Up")]
    internal class PickUp : ObjectTutorial
    {
        /// <summary>
        /// Wait until spawned object is picked up
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            GameObject pickUp = objectSpawner.SpawnedObject;
            yield return new WaitUntil(() => !pickUp.activeSelf);
        }
    }
}
