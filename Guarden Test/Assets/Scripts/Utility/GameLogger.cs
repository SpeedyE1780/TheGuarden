using UnityEngine;

public static class GameLogger
{
    public static void LogInfo(string message, Object sender)
    {
        Debug.Log(message, sender);
    }
}
