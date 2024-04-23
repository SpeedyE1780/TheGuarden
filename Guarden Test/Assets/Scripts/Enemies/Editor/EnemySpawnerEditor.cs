using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Enemies.Editor
{
    internal class EnemySpawnerEditor
    {
        [MenuItem("CONTEXT/EnemySpawner/Autofill Variables")]
        internal static void AutofillVariables(MenuCommand command)
        {
            EnemySpawner spawner = command.context as EnemySpawner;
            RecordEditorHistory.RecordHistory(spawner, $"Autofill {spawner.name} Variables", spawner.AutofillVariables);
        }
    }
}
