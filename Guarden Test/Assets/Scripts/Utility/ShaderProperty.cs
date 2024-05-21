using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// ShaderProperty is a Shader implementation of ExposedProperty
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Utility/Exposed Properties/Shader")]
    public sealed class ShaderProperty : ExposedProperty
    {
        protected override ConvertPropertyToId PropertyToId => Shader.PropertyToID;
    }
}
