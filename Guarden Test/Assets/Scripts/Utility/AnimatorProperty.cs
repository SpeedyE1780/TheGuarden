using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// AnimatorProperty is an Animator implementation of ExposedProperty
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Exposed Properties/Animator")]
    public sealed class AnimatorProperty : ExposedProperty
    {
        protected override ConvertPropertyToId PropertyToId => Animator.StringToHash;
    }
}
