using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    public static class ListExtensions
    {
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}
