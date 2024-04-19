using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// DayLightCycle rotates the directional light to give a day and night cycle
    /// </summary>
    public class DayLightCycle : MonoBehaviour
    {
        [SerializeField, Tooltip("Autofilled. GameTime in scene")]
        private GameTime gameTime;

        void LateUpdate()
        {
            float zAngle = Mathf.Lerp(180, -180, gameTime.DayEndProgress);
            transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }

#if UNITY_EDITOR
        internal void AutofillGameTime()
        {
            gameTime = FindObjectOfType<GameTime>();

            if (gameTime == null)
            {
                GameLogger.LogWarning("Game Time not available in scene", gameObject, GameLogger.LogCategory.Scene);
            }
        }
#endif
    }
}
