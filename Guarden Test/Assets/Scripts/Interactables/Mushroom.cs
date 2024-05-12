using System.Collections.Generic;
using TheGuarden.PlantPowerUps;
using TheGuarden.UI;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.AI;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// Mushroom represent a plant tha can be planted and have power ups
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Mushroom : MonoBehaviour, IPickUp, IInventoryItem, IPoolObject
    {
        [SerializeField, Tooltip("Autofilled. List of active power ups")]
        private List<PlantPowerUp> powerUps = new List<PlantPowerUp>();
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
        [SerializeField, Tooltip("Non Plantable area layer mask")]
        private LayerMask nonPlantableArea;
        [SerializeField, Tooltip("Mushrooms layer mask")]
        private LayerMask mushroomLayerMask;
        [SerializeField, Tooltip("Game event called when mushroom is planted in soil")]
        private GameEvent onPlantInSoil;
        [SerializeField, Tooltip("Game event called when fully grown mushroom is planted")]
        private GameEvent onPlant;
        [SerializeField, Tooltip("Mushroom health component")]
        private Health health;
        [SerializeField, Tooltip("Pool this mushroom belongs to")]
        private ObjectPool<Mushroom> pool;
        [SerializeField, Tooltip("Instruction shown when near plant bed")]
        private InteractionInstruction plantBedInstruction;
        [SerializeField, Tooltip("Mushroom info shown in tutorial and inventory")]
        private MushroomInfo mushroomInfo;
        [SerializeField, Tooltip("Game event called when mushroom is picked up")]
        private TGameEvent<MushroomInfo> onMushroomInfoPickedUp;
        [SerializeField, Tooltip("Collisions gameobject showing whether or not mushroom is colliding with existing mushroom")]
        private GameObject collisionsRadius;
        [SerializeField, Tooltip("Game event called when trying to plant mushroom")]
        private TGameEvent<bool> onPlantingMushroom;

        private PlantSoil plantSoil;

        public Rigidbody Rigidbody => rb;
        public float GrowthPercentage => growPlant.GrowthPercentage;
        public bool IsFullyGrown => growPlant.IsFullyGrown;
        public string Name => mushroomInfo.Name;
        public float UsabilityPercentage => GrowthPercentage;
        public ItemUI ItemUI { get; set; }
        public bool IsConsumedAfterInteraction { get; private set; }
        public bool HasInstantPickUp => GrowthPercentage < 0.001 || IsFullyGrown;
        public Sprite Icon => mushroomInfo.Sprite;

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

        /// <summary>
        /// Toggle plant power ups
        /// </summary>
        /// <param name="active"></param>
        private void TogglePowerUps(bool active)
        {
            foreach (PlantPowerUp powerUp in powerUps)
            {
                powerUp.gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// Free plant soil
        /// </summary>
        private void FreePlantSoil()
        {
            if (plantSoil != null)
            {
                plantSoil.IsAvailable = true;
                plantSoil = null;
            }
        }

        /// <summary>
        /// Enable navmesh carving and all plant power ups
        /// </summary>
        private void Plant()
        {
            navMeshObstacle.carving = true;
            TogglePowerUps(true);
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
            FreePlantSoil();
            onMushroomInfoPickedUp.Raise(mushroomInfo);
        }

        /// <summary>
        /// Indicate where mushroom will be planted if in soil or anywhere
        /// </summary>
        public void OnInteractionStarted()
        {
            gameObject.SetActive(true);
            ToggleCollisions(false);

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
            else
            {
                onPlantingMushroom.Raise(true);
                collisionsRadius.SetActive(false); //Hide collision sphere on mushroom being planted
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
        /// Check if plant position is in a restricted area
        /// </summary>
        /// <returns>True if planting in plant bed or outside planting area</returns>
        private bool IsInRestrictedArea()
        {
            bool isRestrictedArea = !Physics.CheckSphere(transform.position, overlapRadius, plantableAreaMask) || Physics.CheckSphere(transform.position, overlapRadius, nonPlantableArea);
            GameLogger.LogError("Can't plant in planting bed or outside planting area", gameObject, GameLogger.LogCategory.Plant);
            return isRestrictedArea;
        }

        /// <summary>
        /// Check if colliding with other mushrooms
        /// </summary>
        /// <returns>True if colliding with other mushroom</returns>
        private bool IsCollidingWithMushrooms()
        {
            foreach (Collider collider in Physics.OverlapSphere(transform.position, overlapRadius, mushroomLayerMask))
            {
                if (collider.attachedRigidbody != null && collider.attachedRigidbody.gameObject != gameObject)
                {
                    GameLogger.LogError("Can't plant mushroom colliding with other mushroom", gameObject, GameLogger.LogCategory.Plant);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Plant fully grown mushroom anywhere
        /// </summary>
        private void PlantAnywhere()
        {
            GameLogger.LogInfo("Plant anywhere", gameObject, GameLogger.LogCategory.InventoryItem);

            if (IsInRestrictedArea() || IsCollidingWithMushrooms())
            {
                return;
            }

            Plant();
            ToggleCollisions(true);
            IsConsumedAfterInteraction = true;
            onPlant.Raise();
            onPlantingMushroom.Raise(false);
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

        private void ResetItemUI()
        {
            ItemUI.ReturnToPool();
            ItemUI = null;
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
                ResetItemUI();
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
            onPlantingMushroom.Raise(false);
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

        /// <summary>
        /// Reset state before entering pool
        /// </summary>
        public void OnEnterPool()
        {
            gameObject.SetActive(false);
            navMeshObstacle.carving = false;
            ToggleCollisions(true);
            TogglePowerUps(false);
            FreePlantSoil();
            growPlant.OnEnterPool();
            transform.SetParent(null);
            rb.constraints = RigidbodyConstraints.None;
            collisionsRadius.SetActive(false);

            if (ItemUI != null)
            {
                ItemUI.ReturnToPool();
                ItemUI = null;
            }
        }

        /// <summary>
        /// Set mushroom active again when exiting pool
        /// </summary>
        public void OnExitPool()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Drop mushroom in scene
        /// </summary>
        public void Drop()
        {
            transform.SetParent(null);
            gameObject.SetActive(true);
            rb.constraints = RigidbodyConstraints.None;
            ToggleCollisions(true);
            TogglePowerUps(false);
            ResetItemUI();
        }

        /// <summary>
        /// Check for interactables around mushroom and return interaction instruction
        /// </summary>
        /// <returns>Instructions show on screen</returns>
        public InteractionInstruction CheckForInteractable()
        {
            if (Physics.CheckSphere(transform.position, overlapRadius, plantBedMask))
            {
                return plantBedInstruction;
            }

            return null;
        }

        public void ToggleCollisionSphere(bool active)
        {
            collisionsRadius.SetActive(active && IsFullyGrown);
        }

#if UNITY_EDITOR
        internal void AutofillVariables()
        {
            rb = GetComponent<Rigidbody>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            growPlant = GetComponent<GrowPlant>();

            Transform powerUpsParent = transform.Find("PowerUps");

            if (powerUpsParent != null)
            {
                powerUps.Clear();

                foreach (Transform behavior in powerUpsParent)
                {
                    powerUps.Add(behavior.GetComponent<PlantPowerUp>());
                }
            }
            else
            {
                GameLogger.LogError("Mushroom doesn't have PowerUps child", this, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
