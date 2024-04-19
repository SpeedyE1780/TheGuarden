using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Enemies.Editor
{
    internal class EnemySpawnerEditor
    {
        [MenuItem("CONTEXT/EnemySpawner/Autofill GameTime")]
        internal static void AutofillGameTime(MenuCommand command)
        {
            EnemySpawner spawner = command.context as EnemySpawner;
            RecordEditorHistory.RecordHistory(spawner, $"Set {spawner.name} GameTime", spawner.AutofillGameTime);
        }
    }
}
