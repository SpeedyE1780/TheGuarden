using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// ExposedProperty replaces https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@7.1/manual/ExposedPropertyHelper.html
    /// </summary>
    public abstract class ExposedProperty : ScriptableObject
    {
        protected delegate int ConvertPropertyToId(string name);

        [SerializeField, Tooltip("Name of property in graph")]
        private string propertyName;

        private int propertyID = -1;

        protected abstract ConvertPropertyToId PropertyToId { get; }

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
                    propertyID = PropertyToId(propertyName);
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
