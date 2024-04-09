using UnityEngine;

public class PlantingIndicator : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Vector3 localPosition;

    public LayerMask Mask { get; set; }
    public bool PlantingInSoil { get; set; }

    public void UpdateMesh(Mesh mesh, Material[] materials)
    {
        meshFilter.mesh = mesh;
        meshRenderer.materials = materials;
    }

    private void LateUpdate()
    {
        meshRenderer.enabled = PlantingInSoil || !Physics.CheckSphere(transform.position, 2.0f, Mask);
    }

    private void OnDisable()
    {
        transform.localPosition = localPosition;
        PlantingInSoil = false;
    }

    private void OnValidate()
    {
        localPosition = transform.localPosition;
    }
}
