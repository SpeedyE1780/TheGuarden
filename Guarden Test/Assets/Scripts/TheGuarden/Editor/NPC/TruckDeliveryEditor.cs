using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.NPC.Editor
{
    internal class TruckDeliveryEditor
    {
        private static void AutofillVariables<T>(TruckDelivery<T> truckDelivery)
        {
            RecordEditorHistory.RecordHistory(truckDelivery, $"Autofill {truckDelivery.name} GameTime", truckDelivery.AutofillVariables);
        }

        [MenuItem("CONTEXT/MushroomDelivery/Autofill Variables")]
        internal static void MushroomAutofillVariables(MenuCommand command)
        {
            AutofillVariables(command.context as MushroomDelivery);
        }
    }
}
