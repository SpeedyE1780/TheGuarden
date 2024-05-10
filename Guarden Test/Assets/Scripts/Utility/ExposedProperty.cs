using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// ExposedProperty replaces https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@7.1/manual/ExposedPropertyHelper.html
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Exposed Property")]
    public class ExposedProperty : ScriptableObject
    {
        [SerializeField, Tooltip("Name of property in graph")]
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
