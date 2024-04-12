using UnityEngine;

public class AttractBehavior : PlantBehavior
{
    [SerializeField]
    private float attractionAreaRadius;

#if UNITY_EDITOR
    Vector3 destinationDebug;
#endif

    public override void ApplyBehavior(Animal animal)
    {
        GameLogger.LogInfo(animal.name + " Attracted", gameObject, GameLogger.LogCategory.PlantBehaviour);
        animal.SetDestination(GetDestination());
    }

    private Vector3 GetDestination()
    {
        Vector3 destination = transform.position + Random.insideUnitSphere * attractionAreaRadius;
        destination.y = 0;

#if UNITY_EDITOR
        destinationDebug = destination;
#endif

        return destination;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, powerUpRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attractionAreaRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destinationDebug, 0.2f);
    }
}
