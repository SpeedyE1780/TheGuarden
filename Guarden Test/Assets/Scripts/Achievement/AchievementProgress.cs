using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Achievements/Achievement Progress")]
public class AchievementProgress : ScriptableObject
{
    public delegate void ValueChanged(int value);
    private int count = 0;
    public event ValueChanged OnValueChanged;

    public void IncreaseCount(int amount)
    {
        count += amount;

        if(amount > 0)
        {
            OnValueChanged?.Invoke(count);
        }
    }
}
