using TheGuarden.Utility;
using TheGuarden.Utility.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DayLightCycleWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset visualTreeAsset = default;

    private Slider progress;
    private ObjectField dayLightCycle;

    [MenuItem("The Guarden/Tools/Day Light Cycle Window")]
    public static void OpenWindow()
    {
        DayLightCycleWindow wnd = GetWindow<DayLightCycleWindow>();
        wnd.titleContent = new GUIContent("Day Light Cycle Window");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement uxmlTree = visualTreeAsset.Instantiate();
        root.Add(uxmlTree);
        dayLightCycle = rootVisualElement.Query<ObjectField>("DayLightCycleField");
        progress = rootVisualElement.Query<Slider>("ProgressSlider");

        dayLightCycle.value = FindObjectOfType<DayLightCycle>();
    }

    private void Update()
    {
        DayLightCycle cycle = dayLightCycle.value as DayLightCycle;

        if (cycle != null)
        {
            cycle.UpdateLight(progress.value / 24.0f);
        }
    }
}
