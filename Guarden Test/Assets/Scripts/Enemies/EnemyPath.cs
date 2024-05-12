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
        internal int CurrentIndex { get; set; }
        internal Vector3 CurrentPosition => points[CurrentIndex].position;
        internal bool ReachedEndOfPath => CurrentIndex >= points.Count;

#if UNITY_EDITOR
        [SerializeField, Tooltip("Editor Only used to set generated collider width")]
        internal float pathWidth = 1.0f;
        internal List<Transform> Points => points;
#endif
    }
}
