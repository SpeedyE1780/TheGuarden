using UnityEngine;

public class PlantBed : MonoBehaviour
{
    [SerializeField]
    private Color dryColor;
    [SerializeField]
    private Color wetColor;
    [SerializeField]
    private float dryingSpeed = 0.25f;

    private float dryWetRatio = 0;
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        material.color = Color.Lerp(dryColor, wetColor, dryWetRatio);
    }

    private void Update()
    {
        dryWetRatio = Mathf.Clamp01(dryWetRatio - dryingSpeed * Time.deltaTime);
        material.color = Color.Lerp(dryColor, wetColor, dryWetRatio);
    }

    public void Water(float bucketRestoration)
    {
        dryWetRatio = Mathf.Clamp01(dryWetRatio + bucketRestoration);
    }
}
