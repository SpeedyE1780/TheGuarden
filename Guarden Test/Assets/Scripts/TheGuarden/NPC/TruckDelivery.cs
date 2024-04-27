using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheGuarden.Utility;
using System.Runtime.CompilerServices;

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
        [SerializeField, Tooltip("Items Scriptable Objects")]
        private DeliveryItems items;
        [SerializeField, Tooltip("Autofilled. GameTime in scene")]
        protected GameTime gameTime;
        [SerializeField, Tooltip("Autofilled. Camera following players")]
        private FollowTarget followCamera;
        [SerializeField, Tooltip("Transform that deliveries should be aimed at")]
        private Transform deliveryLocation;
        [SerializeField, Tooltip("Delivery Configuration Scriptable Object")]
        private DeliveryConfiguration configuration;
        [SerializeField, Range(0.1f, 0.9f), Tooltip("Percentage of road travelled before items are delivered")]
        private float travelledPercentageDelay = 0.4f;
        [SerializeField, Tooltip("Audio Source played when items are delivered")]
        private AudioSource deliverySource;

        public UnityEvent<int> OnDelivery;

        private bool delivered = false;
        private int deliveryCooldown = 0;

        private Vector3 SpawnPoint => transform.position + Vector3.up;

        private void Start()
        {
            if (configuration.dayOneDelivery)
            {
                StartDelivery();
            }
            else
            {
                deliveryCooldown = configuration.daysBetweenDelivery;
            }

            deliverySource.clip = configuration.audioClip;
            items = items.Clone();
        }

        private void OnEnable()
        {
            gameTime.OnDayEnded += OnDayEnded;
        }

        private void OnDisable()
        {
            gameTime.OnDayEnded -= OnDayEnded;
        }

        /// <summary>
        /// Check if a delivery will occur today if not decrement cooldown
        /// </summary>
        private void OnDayEnded()
        {
            if (deliveryCooldown <= 0)
            {
                StartDelivery();
            }

            deliveryCooldown -= 1;
            items.OnDayEnded();
        }

        /// <summary>
        /// Start coroutine and reset cooldown
        /// </summary>
        private void StartDelivery()
        {
            StartCoroutine(Delivery());
            //Add one to cancel this day's ending contribution
            deliveryCooldown = configuration.daysBetweenDelivery + 1;
        }

        /// <summary>
        /// Loop through delivery hours and start a delivery for each hour
        /// </summary>
        /// <returns></returns>
        private IEnumerator Delivery()
        {
            foreach (int deliveryHour in configuration.hours)
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

                        if (configuration.stopForDelivery)
                        {
                            yield return DeliverItems();
                        }
                        else
                        {
                            StartCoroutine(DeliverItems());
                        }
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
            foreach (GameObject guaranteed in items.Guaranteed)
            {
                yield return SpawnAndConfigureItem(guaranteed);
            }

            if (items.Random.Count > 0)
            {
                GameLogger.LogInfo("RANDOM DELIVERY", this, GameLogger.LogCategory.Plant);
                for (int i = 0; i < items.count; i++)
                {
                    yield return SpawnAndConfigureItem(items.Random.GetRandomItem());
                }
            }
        }

        /// <summary>
        /// Spawn and Configure Item and wait
        /// </summary>
        /// <param name="prefab">Item to spawn</param>
        /// <returns></returns>
        private IEnumerator SpawnAndConfigureItem(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, SpawnPoint, Quaternion.identity);
            Item item = go.GetComponent<Item>();
            ConfigureItem(item);
            yield return new WaitForSeconds(configuration.itemsInterval);
        }

        /// <summary>
        /// Configure spawned item
        /// </summary>
        protected abstract void ConfigureItem(Item item);

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
