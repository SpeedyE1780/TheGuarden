using UnityEngine;

public class ForceFieldBehavior : PlantBehavior
{
    public override void ApplyBehavior(Animal animal)
    {
        Debug.Log(animal.name + " in force field");
        animal.InsideForceField = true;
    }

    public override void RemoveBehavior(Animal animal)
    {
        Debug.Log(animal.name + " out of force field");
        animal.InsideForceField = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, behaviorRange);
    }
}
