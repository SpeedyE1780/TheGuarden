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
            RecordEditorHistory.RecordHistory(growPlant, $"Autofill {growPlant.name} GameTime", growPlant.AutofillVariables);
        }

        [MenuItem("CONTEXT/GrowPlant/Fix Behavior Scale")]
        internal static void FixScale(MenuCommand command)
        {
            GrowPlant growPlant = command.context as GrowPlant;

            Transform behaviorParent = growPlant.GetBehaviorParent();

            if(behaviorParent != null )
            {
                RecordEditorHistory.RecordHistory(behaviorParent, $"Set {growPlant.name} behavior parent global scale to 1", () =>
                {
                    behaviorParent.localScale = new Vector3(1 / growPlant.MaxSize.x, 1 / growPlant.MaxSize.y, 1 / growPlant.MaxSize.z);
                });
            }
        }
    }
}
