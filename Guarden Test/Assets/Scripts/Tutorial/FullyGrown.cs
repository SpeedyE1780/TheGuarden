using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// FullyGrown waits until spawned mushroom is fully grown
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Fully Grown")]
    internal class FullyGrown : Tutorial
    {
        [SerializeField]
        private GrowingInfo tutorialGrowingInfo;

        private bool fullyGrown = false;

        /// <summary>
        /// Called when mushroom is fully grown
        /// </summary>
        public void OnFullyGrown()
        {
            fullyGrown = true;
        }

        /// <summary>
        /// Get gametime and grow and listen to OnFullyGrown
        /// </summary>
        internal override void Setup()
        {
            fullyGrown = false;
            tutorialGrowingInfo.OnNightStarted();
        }

        /// <summary>
        /// Wait until spawned mushroom is fully grown
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            tutorialGrowingInfo.OnDayStarted();
            yield return new WaitUntil(() => fullyGrown);
        }
    }
}
