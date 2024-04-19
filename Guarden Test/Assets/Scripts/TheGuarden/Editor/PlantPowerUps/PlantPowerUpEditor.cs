using UnityEditor;
using UnityEngine;

namespace TheGuarden.PlantPowerUps.Editor
{
    internal class PlantPowerUpEditor
    {
        [MenuItem("CONTEXT/PlantPowerUp/Update Sphere Colliders")]
        internal static void UpdateSphereColliders(MenuCommand command)
        {
            PlantPowerUp powerUp = command.context as PlantPowerUp;
            foreach(SphereCollider collider in powerUp.GetComponents<SphereCollider>())
            {
                collider.radius = powerUp.PowerUpRange;
            }
        }
    }
}
