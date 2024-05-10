using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// Spawns the assigned enemy prefab
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Factories/Enemy")]
    internal class EnemyFactory : SimpleFactory<Enemy> { }
}
