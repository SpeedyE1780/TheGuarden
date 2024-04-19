using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheGuarden.Players.Editor
{
    internal class PlayerMovementEditor
    {
        [MenuItem("CONTEXT/PlayerMovement/Set Rigidbody")]
        internal static void SetInputModule(MenuCommand command)
        {
            PlayerMovement movement = command.context as PlayerMovement;
            movement.SetRigidBody();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
