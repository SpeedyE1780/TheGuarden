using UnityEngine;
using TheGuarden.UI;
using TheGuarden.Utility;

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

    private int remainingUses = 0;

    public string Name => name;
    public float UsabilityPercentage => remainingUses / (float)maxUses;
    public ItemUI ItemUI { get; set; }
    public bool HasInstantPickUp => true;

    /// <summary>
    /// Add water from lake
    /// </summary>
    private void AddWater()
    {
        remainingUses = Mathf.Clamp(remainingUses + 1, 0, maxUses);
    }

    /// <summary>
    /// Water plant bed
    /// </summary>
    /// <param name="plantBed">Plant bed that will be watered</param>
    private void WaterPlantBed(PlantBed plantBed)
    {
        if (remainingUses == 0)
        {
            GameLogger.LogError("Not enough water to water plant bed", gameObject, GameLogger.LogCategory.InventoryItem);
            return;
        }

        plantBed.Water(bucketRestoration);
        remainingUses = Mathf.Clamp(remainingUses - 1, 0, maxUses);
        ItemUI.SetProgress(UsabilityPercentage);
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
        Collider[] plantBedsCollider = new Collider[1];
        int plantBedCount = Physics.OverlapSphereNonAlloc(transform.position, overlapRadius, plantBedsCollider, plantBedMask);

        if (plantBedCount == 0)
        {
            GameLogger.LogError("No plant bed near bucket", gameObject, GameLogger.LogCategory.InventoryItem);
            return;
        }

        PlantBed plantBed = plantBedsCollider[0].GetComponent<PlantBed>();
        WaterPlantBed(plantBed);
        GameLogger.LogInfo("Watering plant bed", gameObject, GameLogger.LogCategory.InventoryItem);
    }

    /// <summary>
    /// Hide bucket
    /// </summary>
    public void OnInteractionCancelled()
    {
        gameObject.SetActive(false);
    }
}
