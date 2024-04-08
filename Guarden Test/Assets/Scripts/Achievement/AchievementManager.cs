using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private List<Achievement> achievements;

    private void Start()
    {
        foreach (Achievement achievement in achievements)
        {
            achievement.Initialize();
        }
    }

    private void OnDestroy()
    {
        foreach (Achievement achievement in achievements)
        {
            achievement.Deinitialize();
        }
    }
}
