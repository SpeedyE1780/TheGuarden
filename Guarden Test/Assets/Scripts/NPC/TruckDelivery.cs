using System.Collections;
using System.Collections.Generic;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// TruckDelivery is a game object that will delivery an Item at a specified time in the day
    /// </summary>
    /// <typeparam name="Item">Type of item that will be spawned and delivered</typeparam>
    [RequireComponent(typeof(AudioSource))]
    internal abstract class TruckDelivery<Item> : MonoBehaviour where Item : MonoBehaviour, IPoolObject
    {
        [SerializeField, Tooltip("Roads on which truck can spawn")]
        private List<RoadLane> roads;
        [SerializeField, Tooltip("Speed of truck")]
        private float speed;
        [SerializeField, Tooltip("Meshes parent")]
        private GameObject meshes;
        [SerializeField, Tooltip("Items Scriptable Objects")]
        private DeliveryItems<Item> items;
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
        [SerializeField, Tooltip("Game event called when items are delivered")]
        private IntGameEvent onItemsDelivered;

        private bool delivered = false;
        private int deliveryCooldown = 0;

        private Vector3 SpawnPoint => transform.position + Vector3.up;

        private void Awake()
        {
            items = items.Clone();
            deliverySource.clip = configuration.audioClip;

            //If no delivery on first day update cooldown to prevent delivery
            if (!configuration.dayOneDelivery)
            {
                SetDeliveryCooldown(configuration.daysBetweenDelivery);
            }
        }

        /// <summary>
        /// Update delivery cooldown
        /// </summary>
        /// <param name="value">New cooldown</param>
        private void SetDeliveryCooldown(int value)
        {
            //Add one to cancel this day's ending contribution
            deliveryCooldown = value + 1;
        }

        /// <summary>
        /// Check if a delivery will occur today if not decrement cooldown
        /// </summary>
        public void OnDayStarted()
        {
            items.OnDayStarted();
            DeliverMushrooms();
        }

        /// <summary>
        /// Try to deliver mushrooms if cooldown is completed
        /// </summary>
        public void DeliverMushrooms()
        {
            if (deliveryCooldown <= 0)
            {
                StartDelivery();
            }

            deliveryCooldown -= 1;
        }

        /// <summary>
        /// Start coroutine and reset cooldown
        /// </summary>
        private void StartDelivery()
        {
            StartCoroutine(Delivery());
            SetDeliveryCooldown(configuration.daysBetweenDelivery);
        }

        /// <summary>
        /// Loop through delivery hours and start a delivery for each hour
        /// </summary>
        /// <returns></returns>
        private IEnumerator Delivery()
        {
            GameLogger.LogInfo($"{name} delivering items", this, GameLogger.LogCategory.InventoryItem);
            delivered = false;
            meshes.SetActive(true);
            RoadLane lane = roads.GetRandomItem();
            transform.SetPositionAndRotation(lane.StartPosition, lane.StartRotation);
            float distanceThreshold = lane.Length * travelledPercentageDelay;
            float distanceTravelled = 0;
            followCamera.AddTarget(transform);
            followCamera.AddTarget(deliveryLocation);

            while ((transform.position - lane.EndPosition).sqrMagnitude > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, lane.EndPosition, speed * Time.deltaTime);
                distanceTravelled += speed * Time.deltaTime;

                if (!delivered && distanceTravelled < distanceThreshold)
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

        /// <summary>
        /// Deliver items with interval
        /// </summary>
        /// <returns></returns>
        private IEnumerator DeliverItems()
        {
            int deliveredItems = 0;

            foreach (ObjectPool<Item> guaranteed in items.Guaranteed)
            {
                yield return SpawnAndConfigureItem(guaranteed);
                deliveredItems += 1;
            }

            if (items.Random.Count > 0)
            {
                for (int i = 0; i < items.count; i++)
                {
                    yield return SpawnAndConfigureItem(items.Random.GetRandomItem());
                    deliveredItems += 1;
                }
            }

            onItemsDelivered.Raise(deliveredItems);
        }

        /// <summary>
        /// Spawn and Configure Item and wait
        /// </summary>
        /// <param name="prefab">Item to spawn</param>
        /// <returns></returns>
        private IEnumerator SpawnAndConfigureItem(ObjectPool<Item> objectPool)
        {
            Item item = objectPool.GetPooledObject();
            item.transform.SetPositionAndRotation(SpawnPoint, Quaternion.identity);
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
            followCamera = FindObjectOfType<FollowTarget>();

            if (followCamera == null)
            {
                GameLogger.LogWarning("Follow Camera not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }

            deliverySource = GetComponent<AudioSource>();
        }
#endif
    }
}
