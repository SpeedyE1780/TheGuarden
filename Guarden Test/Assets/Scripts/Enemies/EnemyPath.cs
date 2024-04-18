using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Enemies
{
    [System.Serializable]
    public struct EnemyPath
    {
        [SerializeField]
        private List<Transform> points;
        public int CurrentIndex { get; set; }
        public readonly Vector3 CurrentPosition => points[CurrentIndex].position;
        public readonly Vector3 LastPosition => points[^1].position;
        public readonly bool ReachedEndOfPath => CurrentIndex >= points.Count;
    } 
}
