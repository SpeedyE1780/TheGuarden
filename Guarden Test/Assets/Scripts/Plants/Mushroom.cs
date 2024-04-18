using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheGuarden.Utility;

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
    private LayerMask plantSoilMask;
    [SerializeField]
    private float overlapRadius = 2.0f;
    [SerializeField]
    private LayerMask plantBedMask;
    private PlantSoil plantSoil;

    public string Name => name;
    public bool HasInstantPickUp => GrowthPercentage == 0;
    public float UsabilityPercentage => GrowthPercentage;
    public bool IsConsumedAfterInteraction { get; set; }

#if UNITY_EDITOR
    [SerializeField]
    private Transform behaviorsParent;
#endif

    public float GrowthPercentage => growPlant.GrowthPercentage;
    public bool IsFullyGrown => growPlant.IsFullyGrown;
    public Rigidbody Rigidbody => rb;
    public ItemUI ItemUI { get; set; }

    private void Plant()
    {
        navMeshObstacle.carving = true;

        foreach (PlantPowerUp behavior in behaviors)
        {
            behavior.gameObject.SetActive(true);
        }
    }

    private void PlantInSoil()
    {
        growPlant.PlantInSoil(plantSoil);
        plantSoil.IsAvailable = false;
    }

    public void PickUp(Transform parent)
    {
        gameObject.SetActive(false);
        growPlant.PickUp();
        transform.SetParent(parent);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if(plantSoil != null )
        {
            plantSoil.IsAvailable = true;
            plantSoil = null;
        }
    }

    public void OnInteractionStarted()
    {
        gameObject.SetActive(true);

        if (!IsFullyGrown)
        {
            Collider[] plantSoils = Physics.OverlapSphere(transform.position, overlapRadius, plantSoilMask);

            foreach (Collider soilCollider in plantSoils)
            {
                PlantSoil soil = soilCollider.GetComponent<PlantSoil>();

                if (soil != null && soil.IsAvailable)
                {
                    plantSoil = soil;
                    transform.position = plantSoil.transform.position;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (plantSoil != null)
        {
            transform.position = plantSoil.transform.position;
        }
    }

    public void OnInteractionPerformed()
    {
        IsConsumedAfterInteraction = false;

        if (IsFullyGrown)
        {
            GameLogger.LogInfo("Plant anywhere", gameObject, GameLogger.LogCategory.InventoryItem);

            if (Physics.CheckSphere(transform.position, overlapRadius, plantBedMask))
            {
                GameLogger.LogError("Can't plant in planting bed", gameObject, GameLogger.LogCategory.InventoryItem);
                return;
            }

            Plant();
            IsConsumedAfterInteraction = true;
        }
        else if (plantSoil != null)
        {
            GameLogger.LogInfo("Plant in soil", gameObject, GameLogger.LogCategory.InventoryItem);
            PlantInSoil();
            IsConsumedAfterInteraction = true;
        }

        if (IsConsumedAfterInteraction)
        {
            transform.SetParent(null);
            Destroy(ItemUI.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnInteractionCancelled()
    {
        gameObject.SetActive(false);
        plantSoil = null;
        transform.localPosition = Vector3.zero;
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
