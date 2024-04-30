using UnityEngine;

namespace TheGuarden
{
    /// <summary>
    /// GrowingInfo has all info related to growing
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Mushrooms/Growing Info")]
    internal class GrowingInfo : ScriptableObject
    {
        [Tooltip("Peak rate at which plant grows")]
        internal float peakGrowingRate;
        [Tooltip("Off peak rate at which plant grows")]
        internal float offPeakGrowingRate;
        [Range(0, 1), Tooltip("Minimum ratio needed to grow")]
        internal float minimumDryWetRatio;
        [Tooltip("Size when growing starts")]
        internal Vector3 startSize;
        [Tooltip("Size when growing ends")]
        internal Vector3 maxSize;

        internal float growthRate = 0;

        public void OnDayStarted()
        {
            growthRate = peakGrowingRate;
        }

        public void OnNightStarted()
        {
            growthRate = offPeakGrowingRate;
        }
    }
}
