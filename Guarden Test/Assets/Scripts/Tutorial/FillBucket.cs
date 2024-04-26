using System.Collections;
using TheGuarden.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName ="Scriptable Objects/Tutorial/Fill Bucket")]
    internal class FillBucket : ObjectTutorial
    {
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
