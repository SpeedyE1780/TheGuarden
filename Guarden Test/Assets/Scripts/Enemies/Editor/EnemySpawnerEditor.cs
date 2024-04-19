using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TheGuarden.Enemies.Editor
{
    internal class EnemySpawnerEditor
    {
        [MenuItem("CONTEXT/EnemySpawner/Autofill GameTime")]
        internal static void AutofillGameTime(MenuCommand command)
        {
            EnemySpawner spawner = command.context as EnemySpawner;
            spawner.AutofillGameTime();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
