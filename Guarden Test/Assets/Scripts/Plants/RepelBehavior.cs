using UnityEngine;

public class RepelBehavior : PlantBehavior
{
    [SerializeField]
    private float repelRange;
    [SerializeField]
    private float minimumRange;

#if UNITY_EDITOR
    Vector3 destinationDebug;
#endif

    public override void ApplyBehavior(Animal animal)
    {
        GameLogger.LogInfo(animal.name + " Repeled", gameObject, GameLogger.LogCategory.PlantBehaviour);

        animal.SetDestination(GetDestination(animal.transform.position));
    }

    private Vector3 GetDestination(Vector3 animalPosition)
    {
        Vector3 direction = animalPosition - transform.position;
        Vector3 destination = direction.normalized * Random.Range(minimumRange, repelRange);

#if UNITY_EDITOR
        destinationDebug = destination;
#endif

        return destination;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, behaviorRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, repelRange);
        Gizmos.DrawWireSphere(transform.position, minimumRange + behaviorRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destinationDebug, 0.2f);
    }
}
