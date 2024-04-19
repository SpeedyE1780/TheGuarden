using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheGuarden.NPC.Editor
{
    internal class TruckDeliveryEditor
    {
        private static void AutofillGameTime<T>(TruckDelivery<T> truckDelivery)
        {
            truckDelivery.AutofillGameTime();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
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
