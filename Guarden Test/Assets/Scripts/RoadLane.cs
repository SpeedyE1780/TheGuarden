using UnityEngine;

public class RoadLane : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    private void OnValidate()
    {
        start = transform.Find("Start");
        end = transform.Find("End");

        if (start == null || end == null)
        {
            Debug.LogWarning("Lane has missing start/end point");
        }
    }

    private void OnDrawGizmos()
    {
        if (start != null && end != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(start.position, 0.5f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(end.position, 0.5f); 
        }
    }
}
