using TheGuarden.Interactable;

namespace TheGuarden.NPC
{
    /// <summary>
    /// MushroomDelivery is a specialization of TruckDelivery that delivers mushrooms
    /// </summary>
    internal class MushroomDelivery : TruckDelivery<Mushroom>
    {
        /// <summary>
        /// Throw Mushroom out of truck
        /// </summary>
        protected override void ConfigureItem(Mushroom mushroom)
        {
            mushroom.Rigidbody.velocity = CalculateVelocity();
            mushroom.SetGameTime(gameTime);
        }
    }
}
