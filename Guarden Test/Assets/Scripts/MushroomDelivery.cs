using UnityEngine;

public class MushroomDelivery : TruckDelivery<Mushroom>
{
    protected override void SpawnItem()
    {
        Mushroom mushroom = Instantiate(items[Random.Range(0, items.Count)], SpawnPoint, Quaternion.identity);
        mushroom.Rigidbody.velocity = CalculateVelocity();
    }
}
