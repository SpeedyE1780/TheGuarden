using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Utility
{
    public class WalkOnWater : MonoBehaviour
    {
        public UnityEvent OnWalkOnWater;

        private void OnTriggerEnter(Collider other)
        {
            OnWalkOnWater.Invoke();
        }
    }
}
