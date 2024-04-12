using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour, IInteractable
{
    public string Name => name;

    public float GrowthPercentage => 0.0f;

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    public void OnInteractionPerformed(Inventory inventory)
    {
        throw new System.NotImplementedException();
    }

    public void OnInteractionStarted(Inventory inventory)
    {
        throw new System.NotImplementedException();
    }
}
