using TheGuarden.NPC;
using UnityEngine;

namespace TheGuarden.PlantPowerUps
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class PlantBehavior : PlantPowerUp
    {
        public abstract void ApplyBehavior(Animal animal);
    }
}
