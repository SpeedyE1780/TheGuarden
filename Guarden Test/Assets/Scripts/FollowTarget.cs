using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }

    private void OnValidate()
    {
        if (target != null)
        {
            offset = transform.position - target.position; 
        }
    }
}
