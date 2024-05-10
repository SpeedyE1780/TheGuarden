using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// Random Factory that will randomly pick and spawn an animal prefab
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Factories/Animal")]
    internal class AnimalFactory : RandomFactory<Animal> { }
}
