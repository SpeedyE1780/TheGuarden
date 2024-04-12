using UnityEngine;

public abstract class PlantPowerUp : MonoBehaviour
{
    [SerializeField]
    protected float powerUpRange;
    [SerializeField]
    private SphereCollider powerUpCollider;

    private void OnValidate()
    {
        powerUpCollider = GetComponent<SphereCollider>();
        powerUpCollider.radius = powerUpRange;
    }
}
