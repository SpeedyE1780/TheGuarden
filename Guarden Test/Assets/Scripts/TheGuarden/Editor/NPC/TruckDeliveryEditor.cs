using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.NPC.Editor
{
    internal class TruckDeliveryEditor
    {
        private static void AutofillGameTime<T>(TruckDelivery<T> truckDelivery)
        {
            RecordEditorHistory.RecordHistory(truckDelivery, $"Autofill {truckDelivery.name} GameTime", truckDelivery.AutofillGameTime);
        }

        [MenuItem("CONTEXT/AnimalDelivery/Autofill GameTime")]
        internal static void AnimalAutofillGameTime(MenuCommand command)
        {
            AutofillGameTime(command.context as AnimalDelivery);
        }

        [MenuItem("CONTEXT/MushroomDelivery/Autofill GameTime")]
        internal static void MushroomAutofillGameTime(MenuCommand command)
        {
            AutofillGameTime(command.context as MushroomDelivery);
        }
    }
}
