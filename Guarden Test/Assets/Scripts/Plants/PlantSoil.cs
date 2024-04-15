using UnityEngine;

public class PlantSoil : MonoBehaviour
{
    [SerializeField]
    private PlantBed plantBed;

    public float DryWetRatio => plantBed.DryWetRatio;
}
