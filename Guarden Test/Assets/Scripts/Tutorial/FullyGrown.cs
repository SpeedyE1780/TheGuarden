using System.Collections;
using TheGuarden.Interactable;
using TheGuarden.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/Fully Grown")]
    internal class FullyGrown : ObjectTutorial
    {
        internal override IEnumerator StartTutorial()
        {
            GameTime gameTime = FindObjectOfType<GameTime>();
            gameTime.SetClockScale(1f);
            GrowPlant grow = objectSpawner.SpawnedObject.GetComponent<GrowPlant>();
            bool fullyGrown = false;
            UnityAction onFullyGrown = () => fullyGrown = true;
            grow.OnFullyGrown.AddListener(onFullyGrown);
            yield return new WaitUntil(() => fullyGrown);
            grow.OnFullyGrown.RemoveListener(onFullyGrown);
        }
    }
}
