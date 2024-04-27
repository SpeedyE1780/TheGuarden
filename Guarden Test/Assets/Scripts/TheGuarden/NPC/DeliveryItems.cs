using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.NPC
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Deliveries/Items")]
    internal class DeliveryItems : ScriptableObject
    {
        [SerializeField, Tooltip("Guaranteed items that will be delivered")]
        internal List<GameObject> guaranteed;
        [SerializeField, Tooltip("Random items that might be delivered")]
        internal List<GameObject> random;
        [SerializeField, Tooltip("Random item count")]
        internal int count;

        internal GameObject RandomItem => random[Random.Range(0, random.Count)];
    }
}
