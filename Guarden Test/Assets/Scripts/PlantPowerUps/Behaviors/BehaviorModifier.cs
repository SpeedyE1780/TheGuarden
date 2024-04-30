using System.Collections;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    internal abstract class BehaviorModifier : ScriptableObject
    {
        internal void ApplyBehavior(IBehavior behavior)
        {
            behavior.StartCoroutine(RunBehavior(behavior));
        }

        protected abstract IEnumerator RunBehavior(IBehavior behavior);
    }
}
