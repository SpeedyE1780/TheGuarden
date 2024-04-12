using UnityEngine;

public class ForceFieldBehavior : PlantBehavior
{
    public override void ApplyBehavior(Animal animal)
    {
        GameLogger.LogInfo(animal.name + " in force field", gameObject, GameLogger.LogCategory.PlantBehaviour);
        animal.InsideForceField = true;
    }

    public override void RemoveBehavior(Animal animal)
    {
        GameLogger.LogInfo(animal.name + " out of force field", gameObject, GameLogger.LogCategory.PlantBehaviour);
        animal.InsideForceField = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, behaviorRange);
    }
}
