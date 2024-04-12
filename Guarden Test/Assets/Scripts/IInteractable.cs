public interface IInteractable
{
    public string Name { get; }
    public float GrowthPercentage { get; }

    public void PickUp();
    public void OnInteractionStarted(Inventory inventory);
    public void OnInteractionPerformed(Inventory inventory);
}
