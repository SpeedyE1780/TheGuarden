using UnityEngine;

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
        if (remainingUses > 0)
        {
            plantBed.Water(bucketRestoration);
            remainingUses = Mathf.Clamp(remainingUses - 1, 0, maxUses);
            ItemUI.SetProgress(UsabilityPercentage);
        }
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

    public void OnInteractionStarted(Inventory inventory)
    {
        if (Physics.CheckSphere(transform.position, overlapRadius, lakeLayer))
        {
            GameLogger.LogInfo("Adding water to bucket", gameObject, GameLogger.LogCategory.InventoryItem);
            AddWater();
            ItemUI.SetProgress(UsabilityPercentage);
        }

        GameLogger.LogError("No lake near bucket", gameObject, GameLogger.LogCategory.InventoryItem);
    }

    public void OnInteractionPerformed(Inventory inventory)
    {
        inventory.WaterPlantBed(this);
    }
}
