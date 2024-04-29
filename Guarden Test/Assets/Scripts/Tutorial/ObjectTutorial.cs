using UnityEngine;

namespace TheGuarden.Tutorial
{
    /// <summary>
    /// This tutorial depends on a spawned object in the scene
    /// </summary>
    internal abstract class ObjectTutorial : Tutorial
    {
        [SerializeField, Tooltip("Spawned object that will be used in tutorial")]
        protected ObjectSpawner objectSpawner;
    }
}
