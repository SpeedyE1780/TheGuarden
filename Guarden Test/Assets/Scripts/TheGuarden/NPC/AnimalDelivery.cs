using UnityEngine;

namespace TheGuarden.NPC
{
    public class AnimalDelivery : TruckDelivery<Animal>
    {
        protected override void SpawnItem()
        {
            Animal animal = Instantiate(items[Random.Range(0, items.Count)], SpawnPoint, Quaternion.identity);
            animal.Rigidbody.velocity = CalculateVelocity();
            animal.enabled = false;
            animal.Agent.enabled = false;
        }
    } 
}
