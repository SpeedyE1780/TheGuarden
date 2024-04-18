using UnityEngine;

/// <summary>
/// PlantSoil represents spot in PlantBed that mushroom can be planted in
/// </summary>
public class PlantSoil : MonoBehaviour
{
    [SerializeField, Tooltip("Plant Bed that this soil belongs to")]
    private PlantBed plantBed;

    internal float DryWetRatio => plantBed.dryWetRatio;
    internal bool IsAvailable { get; set; }

    private void Start()
    {
        IsAvailable = true;
    }
}
