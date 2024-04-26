using System.Collections;
using UnityEngine;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/Pick Up")]
    internal class PickUp : ObjectTutorial
    {
        internal override IEnumerator StartTutorial()
        {
            GameObject pickUp = objectSpawner.SpawnedObject;
            yield return new WaitUntil(() => !pickUp.activeSelf);
        }
    }
}
