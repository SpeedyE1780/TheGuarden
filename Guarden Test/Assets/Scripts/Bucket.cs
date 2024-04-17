using UnityEngine;

public class Bucket : MonoBehaviour, IInteractable, IPickUp
{
    [SerializeField]
    private int maxUses = 3;
    [SerializeField]
    private float bucketRestoration = 0.4f;
    private int remainingUses = 0;

    public string Name => name;
    public bool HasInstantPickUp => true;
    public float UsabilityPercentage => remainingUses / (float)maxUses;

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
        }
    }

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    public IInteractable GetInteractableObject()
    {
        return this;
    }

    public void OnInteractionStarted(Inventory inventory)
    {
        inventory.FillWaterBucket(this);
    }

    public void OnInteractionPerformed(Inventory inventory)
    {
        inventory.WaterPlantBed(this);
    }
}
