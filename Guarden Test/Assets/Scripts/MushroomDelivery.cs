using UnityEngine;

public class MushroomDelivery : TruckDelivery<Mushroom>
{
    protected override void SpawnItem()
    {
        Mushroom mushroom = Instantiate(items[Random.Range(0, items.Count)], transform.position, Quaternion.identity);
        mushroom.Rigidbody.velocity = deliveryLocation.position - transform.position;
    }
}
