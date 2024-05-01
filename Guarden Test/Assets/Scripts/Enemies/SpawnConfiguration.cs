using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheGuarden.Enemies
{
    internal class SpawnConfiguration
    {
        internal List<EnemyPath> paths;
        internal Vector3 position;
        internal Quaternion rotation;
        internal UnityAction OnReachShed;
    }
}
