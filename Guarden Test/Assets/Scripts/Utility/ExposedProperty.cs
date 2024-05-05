using UnityEngine;

namespace TheGuarden.Utility
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Exposed Property")]
    public class ExposedProperty : ScriptableObject
    {
        [SerializeField]
        private string propertyName;

        private int propertyID = -1;

        public int PropertyID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    return -1;
                }

                if (propertyID == -1)
                {
                    GameLogger.LogInfo($"Setting ID of {propertyName}", this, GameLogger.LogCategory.Editor);
                    propertyID = Shader.PropertyToID(propertyName);
                }

                return propertyID;
            }
        }

#if UNITY_EDITOR
        internal void ResetPropertyID()
        {
            propertyID = -1;
        }
#endif
    }
}
