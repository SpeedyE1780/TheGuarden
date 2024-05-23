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
        [SerializeField]
        private bool enableOnStart = true;

        /// <summary>
        /// Get spawned object
        /// </summary>
        internal override void Setup()
        {
            pickUp = objectSpawner.SpawnedObject;
            pickUp.SetActive(false);
        }

        /// <summary>
        /// Wait until spawned object is picked up
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            if(enableOnStart) 
            {
                pickUp.SetActive(true);
            }
            yield return new WaitUntil(() => !pickUp.activeSelf);
        }
    }
}
