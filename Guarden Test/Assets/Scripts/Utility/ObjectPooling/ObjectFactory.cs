using UnityEngine;

namespace TheGuarden.Utility
{
    public abstract class ObjectFactory<T> : ScriptableObject
    {
        public abstract T CreateObject();
    }
}
