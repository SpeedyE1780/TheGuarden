using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// Tags is used to replace string litteral with const variables
    /// </summary>
    public static class Tags
    {
        public const string PickUp = "PickUp";
        public const string Enemy = "Enemy";
        public const string Shed = "Shed";

        /// <summary>
        /// Check if game object has any of the given tags
        /// </summary>
        /// <param name="gameObject">Game object who's tag is being checked</param>
        /// <param name="tags">List of tags to check against</param>
        /// <returns>True if game object has any of the given tags</returns>
        public static bool HasTag(GameObject gameObject, params string[] tags)
        {
            foreach (string tag in tags)
            {
                if (gameObject.CompareTag(tag))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
