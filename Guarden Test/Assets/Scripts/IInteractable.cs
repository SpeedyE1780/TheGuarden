public interface IInteractable
{
    public string Name { get; }
    public float UsabilityPercentage { get; }

    public void OnInteractionStarted(Inventory inventory);
    public void OnInteractionPerformed(Inventory inventory);
}
