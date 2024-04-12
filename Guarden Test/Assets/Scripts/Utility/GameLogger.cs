using UnityEngine;

public static class GameLogger
{
    [System.Flags]
    public enum LogCategory
    {
        None = 0,
        Manager = 1,
        Enemy = 1 << 1,
        Plants = 1 << 2,
    }

    public static void LogInfo(string message, Object sender)
    {
        Debug.Log(message, sender);
    }

    public static void LogWarning(string message, Object sender)
    {
        Debug.LogWarning(message, sender);
    }

    public static void LogError(string message, Object sender)
    {
        Debug.LogError(message, sender);
    }
}
