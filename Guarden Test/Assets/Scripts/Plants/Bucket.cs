using UnityEngine;
using TheGuarden.Utility;

public class Bucket : MonoBehaviour, IPickUp, IInventoryItem
{
    [SerializeField]
    private int maxUses = 3;
    [SerializeField]
    private float bucketRestoration = 0.4f;
    [SerializeField]
    private float overlapRadius = 2.0f;
    [SerializeField]
    private LayerMask lakeLayer;
    [SerializeField]
    private LayerMask plantBedMask;
    private int remainingUses = 0;

    public string Name => name;
    public bool HasInstantPickUp => true;
    public float UsabilityPercentage => remainingUses / (float)maxUses;
    public ItemUI ItemUI { get; set; }

    public void AddWater()
    {
        remainingUses = Mathf.Clamp(remainingUses + 1, 0, maxUses);
    }

    public void WaterPlantBed(PlantBed plantBed)
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

    public void PickUp(Transform parent)
    {
        gameObject.SetActive(false);
        transform.SetParent(parent);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public IInventoryItem GetInventoryItem()
    {
        return this;
    }

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

    public void OnInteractionCancelled()
    {
        gameObject.SetActive(false);
    }
}
