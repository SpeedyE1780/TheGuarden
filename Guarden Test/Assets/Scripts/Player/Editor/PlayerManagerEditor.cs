using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheGuarden.Players.Editor
{
    internal class PlayerManagerEditor
    {
        [MenuItem("CONTEXT/PlayerManager/Set Input Module")]
        internal static void SetInputModule(MenuCommand command)
        {
            PlayerManager manager = command.context as PlayerManager;
            manager.FillInputModule();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
