public interface IPickUp
{
    public bool HasInstantPickUp { get; }

    public void PickUp();
    public IInventoryItem GetInventoryItem();
}
