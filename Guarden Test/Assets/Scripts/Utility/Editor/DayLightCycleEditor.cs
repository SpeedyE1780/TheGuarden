using UnityEditor;
using UnityEngine;

namespace TheGuarden.Utility.Editor
{
    public class DayLightCycleEditor : MonoBehaviour
    {
        [MenuItem("CONTEXT/DayLightCycle/Autofill GameTime")]
        internal static void AutofillGameTime(MenuCommand command)
        {
            //DayLightCycle dayLightCycle = command.context as DayLightCycle;
            //RecordEditorHistory.RecordHistory(dayLightCycle, $"Set {dayLightCycle.name} GameTime", dayLightCycle.AutofillGameTime);
        }
    }
}
