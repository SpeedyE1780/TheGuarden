public interface IInteractable
{
    public string Name { get; }
    public bool HasInstantPickUp { get; }
    public float UsabilityPercentage { get; }

    public void PickUp();
    public void OnInteractionStarted(Inventory inventory);
    public void OnInteractionPerformed(Inventory inventory);
}
