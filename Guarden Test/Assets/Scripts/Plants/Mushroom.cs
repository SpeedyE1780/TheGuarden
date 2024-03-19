using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private List<PlantBehavior> behaviors = new List<PlantBehavior>();
    [SerializeField]
    private GrowPlant growPlant;

#if UNITY_EDITOR
    [SerializeField] private Transform behaviorsParent;
#endif

    public float GrowthPercentage => growPlant.GrowthPercentage;
    public bool IsFullyGrown => growPlant.IsFullyGrown;

    public void Plant(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        growPlant.IsGrowing = true;
        gameObject.SetActive(true);

        foreach (PlantBehavior behavior in behaviors)
        {
            behavior.gameObject.SetActive(true);
        }
    }

    public void PickUp()
    {
        gameObject.SetActive(false);
        growPlant.PickUp();
    }

    private void OnValidate()
    {
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
