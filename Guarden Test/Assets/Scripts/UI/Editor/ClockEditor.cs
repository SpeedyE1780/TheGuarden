using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheGuarden.UI.Editor
{
    internal class ClockEditor
    {
        [MenuItem("CONTEXT/Clock/Autofill GameTime")]
        internal static void AutofillGameTime(MenuCommand command)
        {
            Clock clock = command.context as Clock;
            clock.AutofillGameTime();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
