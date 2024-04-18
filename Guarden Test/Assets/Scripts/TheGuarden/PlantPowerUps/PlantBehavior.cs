using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class PlantBehavior : PlantPowerUp
{
    public abstract void ApplyBehavior(Animal animal);
}
