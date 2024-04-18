public interface IInventoryItem
{
    public string Name { get; }
    public float UsabilityPercentage { get; }
    public ItemUI ItemUI { get; set; }

    public void Select()
    {
        if (ItemUI != null)
        {
            ItemUI.Select(); 
        }
    }

    public void Deselect()
    {
        if(ItemUI != null)
        {
            ItemUI.Deselect();
        }
    }

    public void OnInteractionStarted();
    public void OnInteractionPerformed(Inventory inventory);
    public void OnInteractionCancelled();
}
