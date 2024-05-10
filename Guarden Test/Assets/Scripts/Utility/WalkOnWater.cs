using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Detect when player enter lake trigger and send event
    /// </summary>
    internal class WalkOnWater : MonoBehaviour
    {
        [SerializeField, Tooltip("Walk On Water game event")]
        private GameEvent onWalkOnWater;

        private void OnTriggerEnter(Collider other)
        {
            onWalkOnWater.Raise();
        }
    }
}
