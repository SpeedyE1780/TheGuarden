using UnityEngine;

namespace TheGuarden.Tutorial
{
    internal abstract class ObjectTutorial : Tutorial
    {
        [SerializeField, Tooltip("Spawned object that will be used in tutorial")]
        protected ObjectSpawner objectSpawner;
    }
}
