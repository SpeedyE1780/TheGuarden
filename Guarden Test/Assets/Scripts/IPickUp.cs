using UnityEngine;

public interface IPickUp
{
    public bool HasInstantPickUp { get; }

    public void PickUp(Transform parent);
    public IInventoryItem GetInventoryItem();
}
