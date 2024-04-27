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
        protected override void ConfigureItem(Animal animal)
        {
            animal.Rigidbody.velocity = CalculateVelocity();
            animal.enabled = false;
            animal.Agent.enabled = false;
        }
    }
}
