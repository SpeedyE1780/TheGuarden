using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.NPC.Editor
{
    internal class TruckDeliveryEditor
    {
        [MenuItem("CONTEXT/MushroomDelivery/Autofill Variables")]
        internal static void MushroomAutofillVariables(MenuCommand command)
        {
            MushroomDelivery truckDelivery = command.context as MushroomDelivery;
            RecordEditorHistory.RecordHistory(truckDelivery, $"Autofill {truckDelivery.name} GameTime", truckDelivery.AutofillVariables);
        }
    }
}
