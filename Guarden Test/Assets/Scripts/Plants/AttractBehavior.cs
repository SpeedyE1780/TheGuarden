using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttractBehavior : MonoBehaviour
{
    [SerializeField]
    private float attractionAreaRadius;
    [SerializeField]
    private float attractionDistance;
    [SerializeField]
    private SphereCollider attractionCollider;

#if UNITY_EDITOR
    Vector3 destinationDebug;
#endif

    public Vector3 GetDestination()
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
        Gizmos.DrawWireSphere(transform.position, attractionDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attractionAreaRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destinationDebug, 0.2f);
    }

    private void OnValidate()
    {
        attractionCollider = GetComponent<SphereCollider>();
        attractionCollider.radius = attractionDistance;
    }
}
