using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TruckDelivery<Item> : MonoBehaviour
{
    [SerializeField]
    private List<RoadLane> roads;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject mesh;
    [SerializeField]
    protected List<Item> items;
    [SerializeField]
    private int deliveryItemCount = 2;
    [SerializeField]
    private GameTime gameTime;
    [SerializeField]
    protected Transform deliveryLocation;
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
            SpawnItem();
            yield return new WaitForSeconds(deliveryInterval);
        }
    }

    protected abstract void SpawnItem();

    private void OnValidate()
    {
        gameTime = FindObjectOfType<GameTime>();

        if (gameTime == null)
        {
            Debug.LogWarning("Game Time not available in scene");
        }
    }
}
