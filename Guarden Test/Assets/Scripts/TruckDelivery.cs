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

    private void Start()
    {
        StartCoroutine(Delivery());
    }

    private IEnumerator Delivery()
    {
        while (true)
        {
            mesh.SetActive(true);
            RoadLane lane = roads[Random.Range(0, roads.Count)];

            transform.SetPositionAndRotation(lane.StartPosition, lane.StartRotation);
            bool delivered = false;
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

            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator DeliverMushrooms()
    {
        for (int i = 0; i < deliveryItemCount; i++)
        {
            Mushroom mushroom = Instantiate(mushrooms[Random.Range(0, mushrooms.Count)], transform.position, Quaternion.identity);
            Vector3 velocity = deliveryLocation.position - transform.position;
            velocity.y = Random.Range(3.0f, 5.0f);
            mushroom.Rigidbody.velocity = velocity;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
