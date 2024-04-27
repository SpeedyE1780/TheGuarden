using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheGuarden.Utility;

namespace TheGuarden.NPC
{
    /// <summary>
    /// TruckDelivery is a game object that will delivery an Item at a specified time in the day
    /// </summary>
    /// <typeparam name="Item">Type of item that will be spawned and delivered</typeparam>
    internal abstract class TruckDelivery<Item> : MonoBehaviour
    {
        [SerializeField, Tooltip("Roads on which truck can spawn")]
        private List<RoadLane> roads;
        [SerializeField, Tooltip("Speed of truck")]
        private float speed;
        [SerializeField, Tooltip("Meshes parent")]
        private GameObject meshes;
        [SerializeField, Tooltip("List of possible items that can be delivered")]
        protected List<Item> items;
        [SerializeField, Tooltip("Number of items delivered on each delivery")]
        private int deliveryItemCount = 2;
        [SerializeField, Tooltip("Autofilled. GameTime in scene")]
        protected GameTime gameTime;
        [SerializeField, Tooltip("Autofilled. Camera following players")]
        private FollowTarget followCamera;
        [SerializeField, Tooltip("Transform that deliveries should be aimed at")]
        protected Transform deliveryLocation;
        [SerializeField, Tooltip("Interval between delivering each item")]
        private float deliveryInterval = 0.25f;
        [SerializeField, Tooltip("List of hours that truck should spawn and deliver items")]
        private List<int> deliveryHours;
        [SerializeField, Tooltip("Number of days before next delivery")]
        private int daysBetweenDelivery = 0;
        [SerializeField, Tooltip("If true deliver items on first day")]
        private bool dayOneDelivery = true;
        [SerializeField, Range(0.1f, 0.9f), Tooltip("Percentage of road travelled before items are delivered")]
        private float travelledPercentageDelay = 0.4f;
        [SerializeField, Tooltip("Audio Source played when items are delivered")]
        private AudioSource deliverySource;

        public UnityEvent<int> OnDelivery;

        private bool delivered = false;
        private int deliveryCooldown = 0;

        protected Vector3 SpawnPoint => transform.position + Vector3.up;

        private void Start()
        {
            if (dayOneDelivery)
            {
                QueueDelivery();
            }
            else
            {
                deliveryCooldown = daysBetweenDelivery;
            }
        }

        private void OnEnable()
        {
            gameTime.OnDayEnded += QueueDelivery;
        }

        private void OnDisable()
        {
            gameTime.OnDayEnded -= QueueDelivery;
        }

        /// <summary>
        /// Check if a delivery will occur today if not decrement cooldown
        /// </summary>
        private void QueueDelivery()
        {
            if (deliveryCooldown <= 0)
            {
                StartCoroutine(Delivery());
                //Add one to cancel this day's ending contribution
                deliveryCooldown = daysBetweenDelivery + 1;
            }

            deliveryCooldown -= 1;
        }

        /// <summary>
        /// Loop through delivery hours and start a delivery for each hour
        /// </summary>
        /// <returns></returns>
        private IEnumerator Delivery()
        {
            foreach (int deliveryHour in deliveryHours)
            {
                yield return new WaitUntil(() => gameTime.Hour >= deliveryHour);

                delivered = false;
                meshes.SetActive(true);
                RoadLane lane = roads[Random.Range(0, roads.Count)];
                transform.SetPositionAndRotation(lane.StartPosition, lane.StartRotation);
                float distanceThreshold = lane.Length * travelledPercentageDelay;
                followCamera.AddTarget(transform);
                followCamera.AddTarget(deliveryLocation);

                while (transform.position != lane.EndPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, lane.EndPosition, speed * Time.deltaTime);

                    if (!delivered && Vector3.Distance(transform.position, lane.StartPosition) >= distanceThreshold)
                    {
                        delivered = true;
                        deliverySource.Play();
                        StartCoroutine(DeliverItems());
                        OnDelivery?.Invoke(deliveryItemCount);
                    }

                    yield return null;
                }

                meshes.SetActive(false);
                followCamera.RemoveTarget(transform);
                followCamera.RemoveTarget(deliveryLocation);
            }
        }

        /// <summary>
        /// Deliver items with interval
        /// </summary>
        /// <returns></returns>
        private IEnumerator DeliverItems()
        {
            for (int i = 0; i < deliveryItemCount; i++)
            {
                SpawnItem();
                GameLogger.LogInfo($"{name} delivered item", this, GameLogger.LogCategory.Scene);
                yield return new WaitForSeconds(deliveryInterval);
            }
        }

        /// <summary>
        /// Spawn and throw item out of truck
        /// </summary>
        protected abstract void SpawnItem();

        /// <summary>
        /// Calculated needed velocity to reach deliveryLocation and add an arc
        /// </summary>
        /// <returns>Velocity needed to reach deliveryLocation with arc</returns>
        protected Vector3 CalculateVelocity()
        {
            Vector3 velocity = deliveryLocation.position - SpawnPoint;
            velocity.y = Random.Range(2.0f, 5.0f);
            return velocity;
        }

#if UNITY_EDITOR
        internal void AutofillVariables()
        {
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("Game Time not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }

            followCamera = FindObjectOfType<FollowTarget>();

            if (followCamera == null)
            {
                GameLogger.LogWarning("Follow Camera not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }

            deliverySource = GetComponent<AudioSource>();

            if (deliverySource == null)
            {
                GameLogger.LogWarning($"{name} has no audio source component", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
