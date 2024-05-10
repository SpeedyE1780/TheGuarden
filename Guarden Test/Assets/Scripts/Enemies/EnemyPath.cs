using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// EnemyPath is a list of points that the enemy will follow before leaving the scene
    /// </summary>
    [System.Serializable]
    internal struct EnemyPath
    {
        [SerializeField, Tooltip("Points making the path")]
        private List<Transform> points;
        internal int CurrentIndex { get; set; }
        internal readonly Vector3 CurrentPosition => points[CurrentIndex].position;
        internal readonly bool ReachedEndOfPath => CurrentIndex >= points.Count;

#if UNITY_EDITOR
        internal List<Transform> Points => points;
#endif
    }
}
