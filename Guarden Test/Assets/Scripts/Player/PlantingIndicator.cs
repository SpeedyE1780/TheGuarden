using UnityEngine;

public class PlantingIndicator : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Vector3 localPosition;

    public void UpdateMesh(Mesh mesh, Material[] materials)
    {
        meshFilter.mesh = mesh;
        meshRenderer.materials = materials;
    }

    private void OnDisable()
    {
        transform.localPosition = localPosition;
    }

    private void OnValidate()
    {
        localPosition = transform.localPosition;
    }
}
