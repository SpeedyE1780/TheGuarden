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
        private GameObject pickUp;

        /// <summary>
        /// Get spawned object
        /// </summary>
        internal override void Setup()
        {
            pickUp = objectSpawner.SpawnedObject;
        }

        /// <summary>
        /// Wait until spawned object is picked up
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            yield return new WaitUntil(() => !pickUp.activeSelf);
        }
    }
}
