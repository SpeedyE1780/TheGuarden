using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField]
    private int threshold;
    [SerializeField]
    private AchievementProgress progress;

    private bool isCompleted = false;

    public void Initialize()
    {
        progress.OnValueChanged += OnProgress;
    }

    public void Deinitialize()
    {
        progress.OnValueChanged -= OnProgress;
    }

    private void OnProgress(int value)
    {
        if (!isCompleted && value >= threshold)
        {
            isCompleted = true;
            Debug.Log($"{name} is Completed");
        }
    }
}
