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
        public int CurrentIndex { get; set; }
        public readonly Vector3 CurrentPosition => points[CurrentIndex].position;
        public readonly Vector3 LastPosition => points[^1].position;
        public readonly bool ReachedEndOfPath => CurrentIndex >= points.Count;

#if UNITY_EDITOR
        public List<Transform> Points => points;
#endif
    }
}
