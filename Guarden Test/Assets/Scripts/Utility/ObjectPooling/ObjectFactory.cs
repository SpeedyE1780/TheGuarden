using UnityEngine;

namespace TheGuarden.Utility
{
    public abstract class ObjectFactory<T> : ScriptableObject where T : Object
    {
        internal abstract T CreateObject();
    }
}
