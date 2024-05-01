using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.Utility
{
    public class WalkOnWater : MonoBehaviour
    {
        public GameEvent onWalkOnWater;

        private void OnTriggerEnter(Collider other)
        {
            onWalkOnWater.Raise();
        }
    }
}
