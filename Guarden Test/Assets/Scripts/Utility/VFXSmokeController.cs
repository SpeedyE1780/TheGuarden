using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXSmokeController : MonoBehaviour
{

    [SerializeField]
    private VisualEffect vfx;
    [SerializeField]
    private string velocityParam;
    [SerializeField]
    private string spawnParam;
    [SerializeField]
     private Rigidbody rb;

    void Update()
    {
        vfx.SetVector3(velocityParam, -rb.velocity);        
        vfx.SetBool(spawnParam, rb.velocity.sqrMagnitude > 1);
    }

}
