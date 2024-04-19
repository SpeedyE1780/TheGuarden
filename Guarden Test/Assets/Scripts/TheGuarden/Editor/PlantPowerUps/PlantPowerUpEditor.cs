using TheGuarden.Utility.Editor;
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
            foreach (SphereCollider collider in powerUp.GetComponents<SphereCollider>())
            {
                RecordEditorHistory.RecordHistory(collider, $"Update {powerUp.name} SphereColliders Radius", () => collider.radius = powerUp.PowerUpRange);
            }
        }
    }
}
