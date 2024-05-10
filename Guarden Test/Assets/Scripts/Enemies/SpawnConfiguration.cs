using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// SpawnConfiguration contains basic enemy info
    /// </summary>
    internal class SpawnConfiguration
    {
        internal List<EnemyPath> paths;
        internal Vector3 position;
        internal Quaternion rotation;
        internal float healthMultiplier;
    }
}
