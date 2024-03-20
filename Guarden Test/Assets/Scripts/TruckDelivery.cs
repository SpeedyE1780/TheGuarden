using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDelivery : MonoBehaviour
{
    [SerializeField]
    private List<RoadLane> roads;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject mesh;
    [SerializeField]
    private List<Mushroom> mushrooms;
    [SerializeField]
    private int deliveryItemCount = 2;
    [SerializeField]
    private GameTime gameTime;
    [SerializeField]
    private Transform deliveryLocation;
    [SerializeField]
    private float deliveryInterval = 0.25f;
    [SerializeField]
    private List<int> deliveryHours;

    private bool delivered = false;

    private void OnEnable()
    {
        gameTime.OnDayEnded += QueueDelivery;
    }

    private void OnDisable()
    {
        gameTime.OnDayEnded -= QueueDelivery;
    }

    private void QueueDelivery()
    {
        StartCoroutine(Delivery());
    }

    private IEnumerator Delivery()
    {
        foreach (int deliveryHour in deliveryHours)
        {
            yield return new WaitUntil(() => gameTime.Hour >= deliveryHour);

            delivered = false;
            mesh.SetActive(true);
            RoadLane lane = roads[Random.Range(0, roads.Count)];
            transform.SetPositionAndRotation(lane.StartPosition, lane.StartRotation);

            while (transform.position != lane.EndPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, lane.EndPosition, speed * Time.deltaTime);

                if (!delivered && Vector3.Distance(transform.position, lane.StartPosition) >= lane.Length * 0.4f)
                {
                    delivered = true;
                    StartCoroutine(DeliverMushrooms());
                }

                yield return null;
            }

            mesh.SetActive(false);
        }
    }

    private IEnumerator DeliverMushrooms()
    {
        for (int i = 0; i < deliveryItemCount; i++)
        {
            Mushroom mushroom = Instantiate(mushrooms[Random.Range(0, mushrooms.Count)], transform.position, Quaternion.identity);
            mushroom.Rigidbody.velocity = deliveryLocation.position - transform.position;
            yield return new WaitForSeconds(deliveryInterval);
        }
    }
}
