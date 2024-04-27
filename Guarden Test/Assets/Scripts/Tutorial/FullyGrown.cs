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
        private GameTime gameTime;
        private GrowPlant grow;
        private bool fullyGrown = false;

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
            gameTime = FindObjectOfType<GameTime>();
            grow = objectSpawner.SpawnedObject.GetComponent<GrowPlant>();
            grow.OnFullyGrown.AddListener(OnFullyGrown);
        }

        /// <summary>
        /// Wait until spawned mushroom is fully grown
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            gameTime.SetClockScale(1f);
            yield return new WaitUntil(() => fullyGrown);
            grow.OnFullyGrown.RemoveListener(OnFullyGrown);
        }
    }
}
