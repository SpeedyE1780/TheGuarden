using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public class RuntimeSet<T> : ScriptableObject
    {
        public List<T> items = new List<T>();

        public int Count => items.Count;
        public T this[int index] => items[index];

        public void Add(T item)
        {
            items.SafeAdd(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                return;
            }

            items.RemoveAt(index);
        }
    }
}
