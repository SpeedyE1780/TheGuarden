using TheGuarden.Utility.Editor;
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
            RecordEditorHistory.RecordHistory(growPlant, $"Autofill {growPlant.name} growing particles", growPlant.AutofillVariables);
        }
    }
}
