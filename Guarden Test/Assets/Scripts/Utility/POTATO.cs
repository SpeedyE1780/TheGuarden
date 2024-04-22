using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class POTATO : MonoBehaviour
{
    [ContextMenu("DO STUFF")]
    public void DO()
    {
        NavMeshObstacle obs = GetComponent<NavMeshObstacle>();
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.center = obs.center;
        collider.size = obs.size;
    }
}
