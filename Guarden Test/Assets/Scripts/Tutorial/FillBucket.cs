using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// FillBucket waits until the spawned bucket is filled
    /// </summary>
    [CreateAssetMenu(menuName ="Scriptable Objects/Tutorials/Fill Bucket")]
    internal class FillBucket : ObjectTutorial
    {
        /// <summary>
        /// Waits until the spawned bucket is filled
        /// </summary>
        /// <returns></returns>
        internal override IEnumerator StartTutorial()
        {
            Bucket bucket = objectSpawner.SpawnedObject.GetComponent<Bucket>();
            bool filled = false;
            UnityAction<bool> onWaterAdded = (bool full) => filled = full;
            bucket.OnWaterAdded.AddListener(onWaterAdded);
            yield return new WaitUntil(() => filled);
            bucket.OnWaterAdded.RemoveListener(onWaterAdded);
        }
    }
}
