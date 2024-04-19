using TheGuarden.Utility.Editor;
using UnityEditor;

namespace TheGuarden.Players.Editor
{
    internal class PlayerMovementEditor
    {
        [MenuItem("CONTEXT/PlayerMovement/Set Rigidbody")]
        internal static void SetInputModule(MenuCommand command)
        {
            PlayerMovement movement = command.context as PlayerMovement;
            RecordEditorHistory.RecordHistory(movement, $"Set {movement.name} RigidBoody", movement.SetRigidBody);
        }
    }
}
