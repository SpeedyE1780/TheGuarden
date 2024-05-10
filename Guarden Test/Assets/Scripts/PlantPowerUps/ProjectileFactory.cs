using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    /// <summary>
    /// ProjectileFactory will choose a random projectile prefab
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Factories/Projectile")]
    internal class ProjectileFactory : RandomFactory<Projectile> { }
}
