using UnityEngine;

public class RoadLane : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    public Vector3 StartPosition => start.position;
    public Quaternion StartRotation => start.rotation;
    public Vector3 EndPosition => end.position;
    public float Length => Vector3.Distance(start.position, end.position);

    private void OnValidate()
    {
        start = transform.Find("Start");
        end = transform.Find("End");

        if (start == null || end == null)
        {
            GameLogger.LogWarning("Lane has missing start/end point", gameObject);
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
