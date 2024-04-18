using UnityEngine;
using TheGuarden.Utility;

public class ForceFieldBehavior : PlantBuff
{
#if UNITY_EDITOR
    [SerializeField]
    private SphereCollider navMeshModifierCollider;
#endif

    private void OnEnable()
    {
        EnemyNavMeshBaker.BakeNavMesh();
    }

    private void OnDisable()
    {
        EnemyNavMeshBaker.BakeNavMesh();
    }

    public override void ApplyBuff(Animal animal)
    {
        GameLogger.LogInfo(animal.name + " in force field", gameObject, GameLogger.LogCategory.PlantBehaviour);
        animal.InsideForceField = true;
    }

    public override void RemoveBuff(Animal animal)
    {
        GameLogger.LogInfo(animal.name + " out of force field", gameObject, GameLogger.LogCategory.PlantBehaviour);
        animal.InsideForceField = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (navMeshModifierCollider != null)
        {
            navMeshModifierCollider.radius = powerUpRange;
        }
    }
#endif

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, powerUpRange);
    }
}
