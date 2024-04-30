using System.Collections;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    [CreateAssetMenu(menuName ="Scriptable Objects/Plant Power Ups/Behaviors/Return")]
    internal class ReturnBehavior : BehaviorModifier
    {
        [SerializeField, Tooltip("Number of waypoints enemy should go back to")]
        private int count = 2;

        protected override IEnumerator RunBehavior(IBehavior behavior)
        {
            behavior.RewindPathProgress(count);
            yield break;
        }
    }
}
