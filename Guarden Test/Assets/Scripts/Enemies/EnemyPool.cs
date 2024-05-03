using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Enemies
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Pools/Enemy")]
    public class EnemyPool : ObjectPool<Enemy> { }
}
