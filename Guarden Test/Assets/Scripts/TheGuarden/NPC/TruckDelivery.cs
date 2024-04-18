using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheGuarden.Utility;

namespace TheGuarden.NPC
{
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
        [SerializeField]
        private int daysBetweenDelivery = 0;

        public UnityEvent<int> OnDelivery;

        private bool delivered = false;
        private int deliveryCooldown = 0;

        protected Vector3 SpawnPoint => transform.position + Vector3.up;

        private void Start()
        {
            QueueDelivery();
        }

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
            if (deliveryCooldown <= 0)
            {
                StartCoroutine(Delivery());
                deliveryCooldown = daysBetweenDelivery + 1;
            }

            deliveryCooldown -= 1;
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
                        OnDelivery?.Invoke(deliveryItemCount);
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

        protected Vector3 CalculateVelocity()
        {
            Vector3 velocity = deliveryLocation.position - SpawnPoint;
            velocity.y = Random.Range(2.0f, 5.0f);
            return velocity;
        }

        private void OnValidate()
        {
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("Game Time not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
    } 
}
