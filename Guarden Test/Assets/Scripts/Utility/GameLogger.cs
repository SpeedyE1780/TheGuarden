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

    [System.Diagnostics.Conditional("GAME_LOGGER_LOG_INFO")]
    public static void LogInfo(string message, Object sender)
    {
        Debug.Log(message, sender);
    }

    [System.Diagnostics.Conditional("GAME_LOGGER_LOG_WARNING")]
    public static void LogWarning(string message, Object sender)
    {
        Debug.LogWarning(message, sender);
    }

    [System.Diagnostics.Conditional("GAME_LOGGER_LOG_ERROR")]
    public static void LogError(string message, Object sender)
    {
        Debug.LogError(message, sender);
    }
}
