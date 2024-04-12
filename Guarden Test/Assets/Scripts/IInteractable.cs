public interface IInteractable
{
    public string Name { get; }
    public float GrowthPercentage { get; }

    public void PickUp();
}
