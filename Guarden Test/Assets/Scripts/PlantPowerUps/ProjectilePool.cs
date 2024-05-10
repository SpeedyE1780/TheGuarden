using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// ProjectilePool where all projectile returns to
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Pools/Projectile")]
    internal class ProjectilePool : ObjectPool<Projectile> { }
}
