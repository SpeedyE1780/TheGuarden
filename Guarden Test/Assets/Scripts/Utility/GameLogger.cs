using UnityEngine;

namespace TheGuarden.Utility
{
    public class GameLogger : MonoBehaviour
    {
        [System.Flags]
        public enum LogCategory
        {
            None = 0,
            Achievements = 1,
            Enemy = 1 << 1,
            PlantPowerUp = 1 << 2,
            Scene = 1 << 3,
            Player = 1 << 4,
            Plant = 1 << 5,
            UI = 1 << 6,
            FileOperations = 1 << 7,
            InventoryItem = 1 << 8,
            Audio = 1 << 9,
            Editor = 1 << 10,
            Tutorial = 1 << 11,
            ObjectPooling = 1 << 12,
            All = -1
        }

        [SerializeField, Tooltip("Active category that will be logged")]
        private LogCategory loggedCategory;

        private static LogCategory enabledCategories = LogCategory.All;

        private void Awake()
        {
            enabledCategories = loggedCategory;
        }

        /// <summary>
        /// Logs info to the console if category is active
        /// </summary>
        /// <param name="message">Message to log on console</param>
        /// <param name="sender">Object logging the message</param>
        /// <param name="category">Category of logged message</param>
        [System.Diagnostics.Conditional("THE_GUARDEN_GAME_LOGGER_LOG_INFO")]
        public static void LogInfo(string message, Object sender, LogCategory category)
        {
            if ((category & enabledCategories) == 0)
            {
                return;
            }

            Debug.Log($"<color=white>[INFO] {message}</color>", sender);
        }

        /// <summary>
        /// Logs warning to the console if category is active
        /// </summary>
        /// <param name="message">Message to log on console</param>
        /// <param name="sender">Object logging the message</param>
        /// <param name="category">Category of logged message</param>
        [System.Diagnostics.Conditional("THE_GUARDEN_GAME_LOGGER_LOG_WARNING")]
        public static void LogWarning(string message, Object sender, LogCategory category)
        {
            if ((category & enabledCategories) == 0)
            {
                return;
            }

            Debug.LogWarning($"<color=yellow>[WARNING] {message}</color>", sender);
        }

        /// <summary>
        /// Logs error to the console if category is active
        /// </summary>
        /// <param name="message">Message to log on console</param>
        /// <param name="sender">Object logging the message</param>
        /// <param name="category">Category of logged message</param>
        [System.Diagnostics.Conditional("THE_GUARDEN_GAME_LOGGER_LOG_ERROR")]
        public static void LogError(string message, Object sender, LogCategory category)
        {
            if ((category & enabledCategories) == 0)
            {
                return;
            }

            Debug.LogError($"<color=red>[ERROR] {message}</color>", sender);
        }
    }
}
