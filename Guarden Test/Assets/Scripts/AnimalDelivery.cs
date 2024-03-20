using UnityEngine;

public class AnimalDelivery : TruckDelivery<Animal>
{
    protected override void SpawnItem()
    {
        Instantiate(items[Random.Range(0, items.Count)], deliveryLocation.position, Quaternion.identity);
    }
}
