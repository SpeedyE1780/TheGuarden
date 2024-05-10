using TheGuarden.UI;
using TheGuarden.Utility;
using TheGuarden.Utility.Events;
using UnityEngine;
using UnityEngine.VFX;

namespace TheGuarden.Interactable
{
    /// <summary>
    /// Bucket used to water plant bed
    /// </summary>
    internal class Bucket : MonoBehaviour, IPickUp, IInventoryItem
    {
        [SerializeField, Tooltip("Uses before bucket needs to be filled again")]
        private int maxUses = 3;
        [SerializeField, Tooltip("Percentage restored when watering plant bed")]
        private float bucketRestoration = 0.4f;
        [SerializeField, Tooltip("Radius used to detect lake/plant bed")]
        private float overlapRadius = 2.0f;
        [SerializeField, Tooltip("Lake layer mask")]
        private LayerMask lakeLayer;
        [SerializeField, Tooltip("Plant bed layer mask")]
        private LayerMask plantBedMask;
        [SerializeField, Tooltip("Splash VFX played when adding water or watering plant bed")]
        private VisualEffect splashPrefab;
        [SerializeField, Tooltip("Game event called when water is added to bucket passes true is filled")]
        private BoolGameEvent onWaterAdded;
        [SerializeField, Tooltip("Game event called when plant bed is watered")]
        private GameEvent onPlantBedWatered;
        [SerializeField, Tooltip("Interaction shown when near lake")]
        private InteractionInstruction addWaterInstruction;
        [SerializeField, Tooltip("Interaction shown when near plant bed")]
        private InteractionInstruction waterPlantBedInstruction;
        [SerializeField, Tooltip("Interatction shown when near plant bed and empty bucket")]
        private InteractionInstruction missingWaterInstruction;
        [SerializeField, Tooltip("Bucket Icon")]
        private Sprite icon;

        private int remainingUses = 0;
        private VisualEffect splash;
        private Collider[] lakeCollider = new Collider[1];
        private Collider[] plantBedCollider = new Collider[1];

        public string Name => "Bucket";
        public float UsabilityPercentage => remainingUses / (float)maxUses;
        public ItemUI ItemUI { get; set; }
        public bool HasInstantPickUp => true;
        public Sprite Icon => icon;

        private void Start()
        {
            splash = Instantiate(splashPrefab);
        }

        /// <summary>
        /// Move splash vfx to position and play it
        /// </summary>
        /// <param name="position">Splash position</param>
        private void PlaySplashVFX(Vector3 position)
        {
            splash.transform.position = position;
            splash.Play();
        }

        /// <summary>
        /// Add water from lake
        /// </summary>
        private void AddWater()
        {
            remainingUses = Mathf.Min(remainingUses + 1, maxUses);
            Physics.OverlapSphereNonAlloc(transform.position, overlapRadius, lakeCollider, lakeLayer);
            PlaySplashVFX(lakeCollider[0].ClosestPoint(transform.position));
            onWaterAdded.Raise(remainingUses == maxUses);
        }

        /// <summary>
        /// Water plant bed
        /// </summary>
        /// <param name="plantBed">Plant bed that will be watered</param>
        /// <param name="closestPoint">Closest point on plant bed collider where splash vfx is positioned</param>
        private void WaterPlantBed(PlantBed plantBed, Vector3 closestPoint)
        {
            if (remainingUses == 0)
            {
                GameLogger.LogError("Not enough water to water plant bed", gameObject, GameLogger.LogCategory.InventoryItem);
                return;
            }

            PlaySplashVFX(closestPoint);
            plantBed.Water(bucketRestoration);
            remainingUses = Mathf.Max(remainingUses - 1, 0);
            ItemUI.SetProgress(UsabilityPercentage);
            onPlantBedWatered.Raise();
        }

        /// <summary>
        /// Pick up bucket and set it to follow parent
        /// </summary>
        /// <param name="parent">Parent of bucket transform</param>
        public void PickUp(Transform parent)
        {
            gameObject.SetActive(false);
            transform.SetParent(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Get item that needs to be added to player inventory
        /// </summary>
        /// <returns>The bucket itself</returns>
        public IInventoryItem GetInventoryItem()
        {
            return this;
        }

        /// <summary>
        /// Try to add water to bucket
        /// </summary>
        public void OnInteractionStarted()
        {
            if (!Physics.CheckSphere(transform.position, overlapRadius, lakeLayer))
            {
                GameLogger.LogError("No lake near bucket", gameObject, GameLogger.LogCategory.InventoryItem);
                return;
            }

            GameLogger.LogInfo("Adding water to bucket", gameObject, GameLogger.LogCategory.InventoryItem);
            AddWater();
            ItemUI.SetProgress(UsabilityPercentage);
        }

        /// <summary>
        /// Try to water plant bed
        /// </summary>
        public void OnInteractionPerformed()
        {
            int plantBedCount = Physics.OverlapSphereNonAlloc(transform.position, overlapRadius, plantBedCollider, plantBedMask);

            if (plantBedCount == 0)
            {
                GameLogger.LogError("No plant bed near bucket", gameObject, GameLogger.LogCategory.InventoryItem);
                return;
            }

            PlantBed plantBed = plantBedCollider[0].GetComponent<PlantBed>();
            WaterPlantBed(plantBed, plantBedCollider[0].ClosestPoint(transform.position));
            GameLogger.LogInfo("Watering plant bed", gameObject, GameLogger.LogCategory.InventoryItem);
        }

        /// <summary>
        /// Hide bucket
        /// </summary>
        public void OnInteractionCancelled()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Drop bucket in position and set the gameobject active
        /// </summary>
        public void Drop()
        {
            transform.SetParent(null);
            gameObject.SetActive(true);
            ItemUI.ReturnToPool();
            ItemUI = null;
        }

        /// <summary>
        /// Check for interactables around bucket and return interaction instruction
        /// </summary>
        /// <returns>Instructions show on screen</returns>
        public InteractionInstruction CheckForInteractable()
        {
            if (Physics.CheckSphere(transform.position, overlapRadius, plantBedMask))
            {
                return remainingUses > 0 ? waterPlantBedInstruction : missingWaterInstruction;
            }

            if (Physics.CheckSphere(transform.position, overlapRadius, lakeLayer))
            {
                return addWaterInstruction;
            }

            return null;
        }
    }
}
