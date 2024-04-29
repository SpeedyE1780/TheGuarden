using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// Configuration of delivery truck
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Deliveries/Configuration")]
    internal class DeliveryConfiguration : ScriptableObject
    {
        [SerializeField, Tooltip("List of hours that truck should spawn and deliver items")]
        internal List<int> hours;
        [SerializeField, Tooltip("Interval between delivering each item")]
        internal float itemsInterval = 0.25f;
        [SerializeField, Tooltip("Number of days before next delivery")]
        internal int daysBetweenDelivery = 0;
        [SerializeField, Tooltip("If true deliver items on first day")]
        internal bool dayOneDelivery = true;
        [SerializeField, Tooltip("If true truck stops moving while delivering items")]
        internal bool stopForDelivery = false;
        [SerializeField, Tooltip("Audio clip played on delivery")]
        internal AudioClip audioClip;
    }
}
