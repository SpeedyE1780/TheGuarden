using TheGuarden.Utility;
using UnityEngine;

namespace TheGuarden.UI
{
    /// <summary>
    /// Pool that stores all ItemUI
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Pooling/Pools/Item UI")]
    public class ItemUIPool : ObjectPool<ItemUI> { }
}
