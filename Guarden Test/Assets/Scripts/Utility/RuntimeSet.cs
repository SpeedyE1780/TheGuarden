using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// List of related element
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public class RuntimeSet<T> : ScriptableObject
    {
        private List<T> items = new List<T>();

        public int Count => items.Count;
        public T this[int index] => items[index];

        /// <summary>
        /// Add item to list
        /// </summary>
        /// <param name="item">Add item to list</param>
        public void Add(T item)
        {
            items.SafeAdd(item);
        }

        /// <summary>
        /// Remove item from list
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            items.Remove(item);
        }
    }
}
