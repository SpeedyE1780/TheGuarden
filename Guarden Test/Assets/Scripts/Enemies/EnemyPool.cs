using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Enemies
{
    /// <summary>
    /// Pool that will store all disabled enemies
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Pools/Enemy")]
    internal class EnemyPool : ObjectPool<Enemy> { }
}
