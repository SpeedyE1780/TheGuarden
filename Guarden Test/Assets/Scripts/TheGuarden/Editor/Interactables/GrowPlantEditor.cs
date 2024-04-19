using UnityEditor;
using UnityEngine;

namespace TheGuarden.Interactable.Editor
{
    internal class GrowPlantEditor : MonoBehaviour
    {
        [MenuItem("CONTEXT/GrowPlant/Autofill Variables")]
        internal static void AutofillVariables(MenuCommand command)
        {
            GrowPlant growPlant = command.context as GrowPlant;
            growPlant.AutofillVariables();
        }

        [MenuItem("CONTEXT/GrowPlant/Validate Variables")]
        internal static void ValidateVariables(MenuCommand command)
        {
            GrowPlant growPlant = command.context as GrowPlant;
            growPlant.ValidateVariables();
        }
    }
}
