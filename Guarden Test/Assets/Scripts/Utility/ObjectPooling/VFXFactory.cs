using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// VFX Simple Factory
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Factories/VFX")]
    internal class VFXFactory : SimpleFactory<PooledVisualEffect> { }
}
