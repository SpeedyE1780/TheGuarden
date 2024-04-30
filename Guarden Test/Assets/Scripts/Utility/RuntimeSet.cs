using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public class RuntimeSet<T> : ScriptableObject
    {
        public List<T> items = new List<T>();

        public void Add(T item)
        {
            items.SafeAdd(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }
    }
}
