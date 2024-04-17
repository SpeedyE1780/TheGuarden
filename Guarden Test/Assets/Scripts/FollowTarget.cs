using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Camera followCamera;
    [SerializeField]
    private List<Transform> targets;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float addedOffset = 15;

    private Vector3 center;
    private float boundsSize;

    public Camera Camera => followCamera;

    public void AddTarget(Transform player)
    {
        targets.Add(player);
    }

    public void RemoveTarget(Transform player)
    {
        targets.Remove(player);
    }

    private void CalculateCenter()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

        foreach (Transform target in targets)
        {
            bounds.Encapsulate(target.position);
        }

        center = bounds.center;
        boundsSize = bounds.size.magnitude;
    }

    private void LateUpdate()
    {
        CalculateCenter();
        float frustrumHeight = boundsSize / Camera.aspect;
        float offsetMultiplier = frustrumHeight * 0.5f / Mathf.Tan(Camera.fieldOfView * 0.5f * Mathf.Deg2Rad) + addedOffset;
        transform.position = center + (offset * offsetMultiplier);
    }

#if UNITY_EDITOR

    [ContextMenu("Normalize Offset")]
    public void NormalizeOffset()
    {
        offset = offset.normalized;
    }

#endif
}
