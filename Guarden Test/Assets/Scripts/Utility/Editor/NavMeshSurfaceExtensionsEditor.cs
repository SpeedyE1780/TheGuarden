using UnityEditor;

namespace TheGuarden.Utility.Editor
{
	internal class NavMeshSurfaceExtensionsEditor
	{
		[MenuItem("CONTEXT/NavMeshSurfaceExtensions/Autofill NavMeshSurafe")]
		internal static void AutofillNavMeshSurface(MenuCommand command)
		{
			NavMeshSurfaceExtensions navMeshSurfaceExtensions = command.context as NavMeshSurfaceExtensions;
			RecordEditorHistory.RecordHistory(navMeshSurfaceExtensions, $"Set {navMeshSurfaceExtensions.name} NavMeshSurface", navMeshSurfaceExtensions.AutofillSurface);

		}
	}
}
