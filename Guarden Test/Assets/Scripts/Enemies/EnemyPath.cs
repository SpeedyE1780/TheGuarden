using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// EnemyPath is a list of points that the enemy will follow before leaving the scene
    /// </summary>
    [System.Serializable]
    internal class EnemyPath : MonoBehaviour
    {
        [SerializeField, Tooltip("Points making the path")]
        private List<Transform> points;

        internal Vector3 this[int index] => points[index].position;
        internal int Count => points.Count;

#if UNITY_EDITOR
        [Header("EDITOR ONLY")]
        [SerializeField, Tooltip("Editor Only used to set generated collider width")]
        internal float pathWidth = 1.0f;
        [SerializeField, Tooltip("Prefab spawned to show restricted planting area")]
        internal GameObject forceFieldPrefab;

        internal List<Transform> Points => points;
#endif
    }
}
