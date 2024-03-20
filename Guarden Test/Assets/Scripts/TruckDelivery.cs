using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckDelivery : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private List<RoadLane> roads;
    [SerializeField]
    private float stoppingDistance;
    [SerializeField]
    private GameObject mesh;

    private void Start()
    {
        StartCoroutine(Delivery());
    }

    IEnumerator Delivery()
    {
        while (true)
        {
            mesh.SetActive(true);
            RoadLane lane = roads[Random.Range(0, roads.Count)];

            transform.SetPositionAndRotation(lane.StartPosition, lane.StartRotation);
            agent.SetDestination(lane.EndPosition);

            yield return new WaitUntil(() => agent.remainingDistance <= stoppingDistance);

            mesh.SetActive(false);

            yield return new WaitForSeconds(1.0f);
        }
    }
}
