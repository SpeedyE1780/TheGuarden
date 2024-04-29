using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// FillBucket waits until the spawned bucket is filled
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorials/Fill Bucket")]
    internal class FillBucket : ObjectTutorial
    {
        private Bucket bucket;
        private bool filled = false;

        /// <summary>
        /// Called when water is added to bucket
        /// </summary>
        /// <param name="full">True if bucket is filled</param>
        private void OnAddWater(bool full)
        {
            filled = full;
        }

        /// <summary>
        /// Get bucket and listen to OnWaterAdded
        /// </summary>
        internal override void Setup()
        {
            filled = false;
            bucket = objectSpawner.SpawnedObject.GetComponent<Bucket>();
            bucket.OnWaterAdded.AddListener(OnAddWater);
        }

        /// <summary>
        /// Waits until the spawned bucket is filled
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {

            yield return new WaitUntil(() => filled);
            bucket.OnWaterAdded.RemoveListener(OnAddWater);
        }
    }
}
