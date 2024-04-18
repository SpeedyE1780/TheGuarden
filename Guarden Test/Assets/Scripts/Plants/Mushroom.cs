using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mushroom : MonoBehaviour, IPickUp, IInventoryItem
{
    [SerializeField]
    private List<PlantPowerUp> behaviors = new List<PlantPowerUp>();
    [SerializeField]
    private GrowPlant growPlant;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private NavMeshObstacle navMeshObstacle;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    public string Name => name;
    public bool HasInstantPickUp => GrowthPercentage == 0;
    public float UsabilityPercentage => GrowthPercentage;
    public Mesh Mesh => meshFilter.mesh;
    public Material[] Materials => meshRenderer.materials;

#if UNITY_EDITOR
    [SerializeField]
    private Transform behaviorsParent;
#endif

    public float GrowthPercentage => growPlant.GrowthPercentage;
    public bool IsFullyGrown => growPlant.IsFullyGrown;
    public Rigidbody Rigidbody => rb;
    public ItemUI ItemUI { get; set; }

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

        foreach (PlantPowerUp behavior in behaviors)
        {
            behavior.gameObject.SetActive(true);
        }
    }

    public void PlantInSoil(PlantSoil plantSoil, Vector3 position, Quaternion rotation)
    {
        InitializePlantedState(position, rotation);
        growPlant.PlantInSoil(plantSoil);
    }

    public void PickUp(Transform parent)
    {
        gameObject.SetActive(false);
        growPlant.PickUp();
        transform.SetParent(parent);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void OnInteractionStarted(Inventory inventory)
    {
        inventory.ShowPlantingIndicator(this);
    }

    public void OnInteractionPerformed(Inventory inventory)
    {
        inventory.PlantMushroom(this);
    }

    public IInventoryItem GetInventoryItem()
    {
        return this;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        if (behaviorsParent != null)
        {
            behaviors.Clear();

            foreach (Transform behavior in behaviorsParent)
            {
                behaviors.Add(behavior.GetComponent<PlantPowerUp>());
            }
        }
    }
#endif
}
