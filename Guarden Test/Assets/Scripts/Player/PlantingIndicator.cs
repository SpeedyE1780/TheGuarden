using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingIndicator : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    public void UpdateMesh(Mesh mesh, Material[] materials)
    {
        meshFilter.mesh = mesh;
        meshRenderer.materials = materials;
    }
}
