using System.Collections;
using TheGuarden.Interactable;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// FullyGrown waits until spawned mushroom is fully grown
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Fully Grown")]
    internal class FullyGrown : ObjectTutorial
    {
        private GrowPlant grow;
        private bool fullyGrown = false;
        [SerializeField]
        private GrowingInfo tutorialGrowingInfo;

        /// <summary>
        /// Called when mushroom is fully grown
        /// </summary>
        private void OnFullyGrown()
        {
            fullyGrown = true;
        }

        /// <summary>
        /// Get gametime and grow and listen to OnFullyGrown
        /// </summary>
        internal override void Setup()
        {
            fullyGrown = false;
            grow = objectSpawner.SpawnedObject.GetComponent<GrowPlant>();
            grow.enabled = false;
            grow.OnFullyGrown.AddListener(OnFullyGrown);
        }

        /// <summary>
        /// Wait until spawned mushroom is fully grown
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            grow.enabled = true;
            tutorialGrowingInfo.OnDayStarted();
            yield return new WaitUntil(() => fullyGrown);
            grow.OnFullyGrown.RemoveListener(OnFullyGrown);
        }
    }
}
