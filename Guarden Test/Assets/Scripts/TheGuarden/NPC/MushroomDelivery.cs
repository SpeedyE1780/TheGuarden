using UnityEngine;
using TheGuarden.Interactable;

namespace TheGuarden.NPC
{
    /// <summary>
    /// MushroomDelivery is a specialization of TruckDelivery that delivers mushrooms
    /// </summary>
    internal class MushroomDelivery : TruckDelivery<Mushroom>
    {
        /// <summary>
        /// /// <summary>
        /// Spawn Mushroom and throw it out of truck
        /// </summary>
        /// </summary>
        protected override void SpawnItem()
        {
            Mushroom mushroom = Instantiate(items[Random.Range(0, items.Count)], SpawnPoint, Quaternion.identity);
            mushroom.Rigidbody.velocity = CalculateVelocity();
        }
    }
}
