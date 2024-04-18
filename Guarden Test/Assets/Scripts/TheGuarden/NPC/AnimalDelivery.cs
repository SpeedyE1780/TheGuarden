using UnityEngine;

namespace TheGuarden.NPC
{
    /// <summary>
    /// AnimalDelivery is a specialization of TruckDelivery that delivers animals
    /// </summary>
    internal class AnimalDelivery : TruckDelivery<Animal>
    {
        /// <summary>
        /// Spawn Animal and throw it out of truck
        /// </summary>
        protected override void SpawnItem()
        {
            Animal animal = Instantiate(items[Random.Range(0, items.Count)], SpawnPoint, Quaternion.identity);
            animal.Rigidbody.velocity = CalculateVelocity();
            animal.enabled = false;
            animal.Agent.enabled = false;
        }
    }
}
