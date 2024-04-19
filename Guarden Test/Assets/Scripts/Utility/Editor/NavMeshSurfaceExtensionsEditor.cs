using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheGuarden.Utility.Editor
{
	internal class NavMeshSurfaceExtensionsEditor
	{
		[MenuItem("CONTEXT/NavMeshSurfaceExtensions/Autofill NavMeshSurafe")]
		internal static void AutofillNavMeshSurface(MenuCommand command)
		{
			NavMeshSurfaceExtensions navMeshSurfaceExtensions = command.context as NavMeshSurfaceExtensions;
			navMeshSurfaceExtensions.AutofillSurface();
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		}
	}
}
