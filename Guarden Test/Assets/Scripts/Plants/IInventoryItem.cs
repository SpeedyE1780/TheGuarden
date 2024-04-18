public interface IInventoryItem
{
    public string Name { get; }
    public float UsabilityPercentage { get; }
    public ItemUI ItemUI { get; set; }
    public bool IsConsumedAfterInteraction => false;

    public void Select()
    {
        if (ItemUI != null)
        {
            ItemUI.Select();
        }
    }

    public void Deselect()
    {
        if (ItemUI != null)
        {
            ItemUI.Deselect();
        }
    }

    public void SetItemUI(ItemUI itemUI)
    {
        ItemUI = itemUI;
        itemUI.SetItem(Name, UsabilityPercentage);
    }

    public void OnInteractionStarted();
    public void OnInteractionPerformed();
    public void OnInteractionCancelled();
}
