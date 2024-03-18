using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class PlantBehavior : MonoBehaviour
{
    [SerializeField]
    protected SphereCollider behaviorCollider;
    [SerializeField]
    protected float behaviorRange;

    public abstract void ApplyBehavior(Animal animal);
    public virtual void RemoveBehavior(Animal animal) { }

    private void OnValidate()
    {
        behaviorCollider = GetComponent<SphereCollider>();
        behaviorCollider.radius = behaviorRange;
    }
}
