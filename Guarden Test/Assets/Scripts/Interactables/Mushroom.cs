using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheGuarden.PlantPowerUps;
using TheGuarden.UI;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// Mushroom represent a plant tha can be planted and have power ups
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(NavMeshObstacle))]
    public class Mushroom : MonoBehaviour, IPickUp, IInventoryItem, IPoolObject
    {
        [SerializeField, Tooltip("Autofilled. List of active power ups")]
        private List<PlantPowerUp> behaviors = new List<PlantPowerUp>();
        [SerializeField, Tooltip("Autofilled. Growing component")]
        private GrowPlant growPlant;
        [SerializeField, Tooltip("Autofilled. Rigidbody component")]
        private Rigidbody rb;
        [SerializeField, Tooltip("Autofilled. NavMeshObstacle component")]
        private NavMeshObstacle navMeshObstacle;
        [SerializeField, Tooltip("Plant soil layer mask")]
        private LayerMask plantSoilMask;
        [SerializeField, Tooltip("Radius used with overlap spheres")]
        private float overlapRadius = 2.0f;
        [SerializeField, Tooltip("Plant bed layer mask")]
        private LayerMask plantBedMask;
        [SerializeField, Tooltip("Plant area layer mask")]
        private LayerMask plantableAreaMask;
        [SerializeField]
        private GameEvent onPlantInSoil;
        [SerializeField]
        private GameEvent onPlant;
        [SerializeField]
        private Health health;
        [SerializeField]
        private ObjectPool<Mushroom> pool;

        private PlantSoil plantSoil;

        public Rigidbody Rigidbody => rb;
        public float GrowthPercentage => growPlant.GrowthPercentage;
        public bool IsFullyGrown => growPlant.IsFullyGrown;
        public string Name => name;
        public float UsabilityPercentage => GrowthPercentage;
        public ItemUI ItemUI { get; set; }
        public bool IsConsumedAfterInteraction { get; private set; }
        public bool HasInstantPickUp => GrowthPercentage < 0.001 || GrowthPercentage == 1;

        private void Start()
        {
            health.OnOutOfHealth = () => pool.AddObject(this);
        }

        /// <summary>
        /// Ignore collisions
        /// </summary>
        /// <param name="active">Whether collisions are active or no</param>
        private void ToggleCollisions(bool active)
        {
            rb.excludeLayers = active ? 0 : ~0;
        }

        private void ToggleBehaviors(bool active)
        {
            foreach (PlantPowerUp behavior in behaviors)
            {
                behavior.gameObject.SetActive(active);
            }
        }

        private void ResetPlantSoil()
        {
            if (plantSoil != null)
            {
                plantSoil.IsAvailable = true;
                plantSoil = null;
            }
        }

        /// <summary>
        /// Enable navmesh carving and all plant behaviors
        /// </summary>
        private void Plant()
        {
            navMeshObstacle.carving = true;
            ToggleBehaviors(true);
        }

        /// <summary>
        /// Reset growing behavior, set mushroom as child of inventory and free plant soil
        /// </summary>
        /// <param name="parent"></param>
        public void PickUp(Transform parent)
        {
            gameObject.SetActive(false);
            growPlant.ResetGrowing();
            transform.SetParent(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            rb.constraints = RigidbodyConstraints.FreezeAll;
            ToggleCollisions(false);
            ResetPlantSoil();
        }

        /// <summary>
        /// Indicate where mushroom will be planted if in soil or anywhere
        /// </summary>
        public void OnInteractionStarted()
        {
            gameObject.SetActive(true);

            if (!IsFullyGrown)
            {
                Collider[] plantSoils = Physics.OverlapSphere(transform.position, overlapRadius, plantSoilMask);

                foreach (Collider soilCollider in plantSoils)
                {
                    PlantSoil soil = soilCollider.GetComponent<PlantSoil>();

                    if (soil != null && soil.IsAvailable)
                    {
                        plantSoil = soil;
                        transform.position = plantSoil.transform.position;
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (plantSoil != null)
            {
                transform.position = plantSoil.transform.position;
            }
        }

        /// <summary>
        /// Plant anywhere except on plant beds
        /// </summary>
        private void PlantAnywhere()
        {
            GameLogger.LogInfo("Plant anywhere", gameObject, GameLogger.LogCategory.InventoryItem);

            if (!Physics.CheckSphere(transform.position, overlapRadius, plantableAreaMask) || Physics.CheckSphere(transform.position, overlapRadius, plantBedMask))
            {
                GameLogger.LogError("Can't plant in planting bed or outside planting area", gameObject, GameLogger.LogCategory.InventoryItem);
                return;
            }

            Plant();
            ToggleCollisions(true);
            IsConsumedAfterInteraction = true;
            onPlant.Raise();
        }

        /// <summary>
        /// Start growing behavior and make soil unavailable for others
        /// </summary>
        private void PlantInSoil()
        {
            GameLogger.LogInfo("Plant in soil", gameObject, GameLogger.LogCategory.InventoryItem);
            growPlant.PlantInSoil(plantSoil);
            ToggleCollisions(true);
            plantSoil.IsAvailable = false;
            IsConsumedAfterInteraction = true;
            onPlantInSoil.Raise();
        }

        /// <summary>
        /// Try to plant mushroom anywhere or in soil
        /// </summary>
        public void OnInteractionPerformed()
        {
            IsConsumedAfterInteraction = false;

            if (IsFullyGrown)
            {
                PlantAnywhere();
            }
            else if (plantSoil != null)
            {
                PlantInSoil();
            }

            if (IsConsumedAfterInteraction)
            {
                transform.SetParent(null);
                ItemUI.ReturnToPool();
                ItemUI = null;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Hide mushroom and reset position if it was placed on a soil
        /// </summary>
        public void OnInteractionCancelled()
        {
            gameObject.SetActive(false);
            plantSoil = null;
            transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Get item that needs to be added to player inventory
        /// </summary>
        /// <returns>The mushroom itself</returns>
        public IInventoryItem GetInventoryItem()
        {
            return this;
        }

        /// <summary>
        /// Stop highlighting item in inventory and cancel interaction if started
        /// </summary>
        public void Deselect()
        {
            if (ItemUI != null)
            {
                ItemUI.Deselect();
            }

            //If mushroom was deselected while interaction was active cancel the interaction
            OnInteractionCancelled();
        }

        public void OnEnterPool()
        {
            gameObject.SetActive(false);
            navMeshObstacle.carving = false;
            ToggleCollisions(true);
            ToggleBehaviors(false);
            growPlant.OnEnterPool();
            transform.SetParent(null);
            rb.constraints = RigidbodyConstraints.None;

            if (ItemUI != null)
            {
                ItemUI.ReturnToPool();
                ItemUI = null;
            }
        }

        public void OnExitPool()
        {
            gameObject.SetActive(true);
            growPlant.OnExitPool();
        }

        public void Drop()
        {
            transform.SetParent(null);
            gameObject.SetActive(true);
            rb.constraints = RigidbodyConstraints.None;
            ToggleCollisions(true);
        }

#if UNITY_EDITOR
        internal void AutofillVariables()
        {
            rb = GetComponent<Rigidbody>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            growPlant = GetComponent<GrowPlant>();

            Transform behaviorsParent = transform.Find("Behaviors");

            if (behaviorsParent != null)
            {
                behaviors.Clear();

                foreach (Transform behavior in behaviorsParent)
                {
                    behaviors.Add(behavior.GetComponent<PlantPowerUp>());
                }
            }
            else
            {
                GameLogger.LogError("Mushroom doesn't have Behaviors child", this, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
