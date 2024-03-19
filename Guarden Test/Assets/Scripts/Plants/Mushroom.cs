using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private List<PlantBehavior> behaviors = new List<PlantBehavior>();
    [SerializeField]
    private GrowPlant growPlant;
    [SerializeField]
    private Rigidbody rb;

#if UNITY_EDITOR
    [SerializeField] private Transform behaviorsParent;
#endif

    public float GrowthPercentage => growPlant.GrowthPercentage;
    public bool IsFullyGrown => growPlant.IsFullyGrown;

    private void InitializePlantedState(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Plant(Vector3 position, Quaternion rotation)
    {
        InitializePlantedState(position, rotation);

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
