using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public static class ListExtensions
    {
        /// <summary>
        /// Get random element in list
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">caller</param>
        /// <returns>Random element in list</returns>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Add element if not in list
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">caller</param>
        /// <param name="item">Item that needs to be added</param>
        /// <returns>True if item was added</returns>
        public static bool SafeAdd<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
                return true;
            }

            return false;
        }
    }
}
