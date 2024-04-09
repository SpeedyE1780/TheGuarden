using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private List<PlantBehavior> behaviors = new List<PlantBehavior>();
    [SerializeField]
    private GrowPlant growPlant;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private NavMeshObstacle navMeshObstacle;
    [SerializeField]
    private Mesh mesh;

    public Mesh Mesh => mesh;

#if UNITY_EDITOR
    [SerializeField] private Transform behaviorsParent;
#endif

    public float GrowthPercentage => growPlant.GrowthPercentage;
    public bool IsFullyGrown => growPlant.IsFullyGrown;

    public Rigidbody Rigidbody => rb;

    private void InitializePlantedState(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Plant(Vector3 position, Quaternion rotation)
    {
        InitializePlantedState(position, rotation);

        navMeshObstacle.carving = true;

        foreach (PlantBehavior behavior in behaviors)
        {
            behavior.gameObject.SetActive(true);
        }
    }

    public void PlantInSoil(Vector3 position, Quaternion rotation)
    {
        InitializePlantedState(position, rotation);
        growPlant.IsGrowing = true;
    }

    public void PickUp()
    {
        gameObject.SetActive(false);
        growPlant.PickUp();
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        if (behaviorsParent != null)
        {
            behaviors.Clear();

            foreach (Transform behavior in behaviorsParent)
            {
                behaviors.Add(behavior.GetComponent<PlantBehavior>());
            }
        }
    }
}
