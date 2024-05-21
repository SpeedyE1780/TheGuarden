using UnityEngine;
using UnityEngine.VFX;

namespace TheGuarden.Utility
{
    /// <summary>
    /// VFXSmokeController is used to update the Visual Effect graph properties
    /// </summary>
    [RequireComponent(typeof(VisualEffect))]
    public class VFXSmokeController : MonoBehaviour
    {
        [SerializeField, Tooltip("Visual Effect component")]
        private VisualEffect vfx;
        [SerializeField, Tooltip("Velocity Param Name")]
        private ShaderProperty velocityParam;
        [SerializeField, Tooltip("Spawn Param Name")]
        private ShaderProperty spawnParam;
        [SerializeField, Tooltip("Rigidbody that Visual Effect is attached to")]
        private Rigidbody rb;

        void Update()
        {
            vfx.SetVector3(velocityParam.PropertyID, -rb.velocity);
            vfx.SetBool(spawnParam.PropertyID, rb.velocity.sqrMagnitude > 1);
        }
    }
}
