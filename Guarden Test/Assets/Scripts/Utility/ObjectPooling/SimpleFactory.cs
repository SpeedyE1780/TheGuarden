using UnityEngine;

namespace TheGuarden.Utility
{
    public class SimpleFactory<T> : ObjectFactory<T> where T : Object
    {
        [SerializeField]
        private T prefab;

        internal override T CreateObject()
        {
            return Instantiate(prefab);
        }
    }
}
