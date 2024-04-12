using UnityEngine;

public class Bucket : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int maxUses = 3;
    private int remainingUses = 0;

    public string Name => name;
    public bool HasInstantPickUp => false;
    public float UsabilityPercentage => remainingUses / (float)maxUses;

    public void AddWater()
    {
        remainingUses = Mathf.Clamp(remainingUses + 1, 0, maxUses);
    }

    public void RemoveWater()
    {
        remainingUses = Mathf.Clamp(remainingUses - 1, 0, maxUses);
    }

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    public void OnInteractionStarted(Inventory inventory)
    {
        inventory.FillWaterBucket(this);
    }

    public void OnInteractionPerformed(Inventory inventory)
    {
        inventory.WaterSoil(this);
    }
}
