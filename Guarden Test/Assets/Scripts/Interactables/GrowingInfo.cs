using UnityEngine;

namespace TheGuarden
{
    /// <summary>
    /// GrowingInfo has all info related to growing
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Mushrooms/Growing Info")]
    internal class GrowingInfo : ScriptableObject
    {
        [SerializeField, Tooltip("Peak rate at which plant grows")]
        internal float peakGrowingRate;
        [SerializeField, Tooltip("Off peak rate at which plant grows")]
        internal float offPeakGrowingRate;
        [SerializeField, Range(0, 1), Tooltip("Minimum ratio needed to grow")]
        internal float minimumDryWetRatio;
        [SerializeField, Tooltip("Size when growing starts")]
        internal Vector3 startSize;
        [SerializeField, Tooltip("Size when growing ends")]
        internal Vector3 maxSize;

        internal float growthRate = 0;

        /// <summary>
        /// Called from OnDayStarted Game Event
        /// </summary>
        public void OnDayStarted()
        {
            growthRate = peakGrowingRate;
        }

        /// <summary>
        /// Called from OnNightStarted Game Event
        /// </summary>
        public void OnNightStarted()
        {
            growthRate = offPeakGrowingRate;
        }
    }
}
