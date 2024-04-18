using UnityEngine;

public class PlantSoil : MonoBehaviour
{
    [SerializeField]
    private PlantBed plantBed;

    public float DryWetRatio => plantBed.DryWetRatio;
    public bool IsAvailable { get; set; }

    private void Start()
    {
        IsAvailable = true;
    }
}
