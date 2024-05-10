using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// Factory that will spawn assigned mushroom prefab
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Factories/Mushroom")]
    internal class MushroomFactory : SimpleFactory<Mushroom>
    {
    }
}
