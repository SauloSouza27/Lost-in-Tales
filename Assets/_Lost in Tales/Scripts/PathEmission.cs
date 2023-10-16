using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEmission : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Material material;

    private void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }

    private void OnCollisionStay(Collision collision)
    {
        material.EnableKeyword("_EMISSION");
    }

    private void OnCollisionExit(Collision collision)
    {
        material.DisableKeyword("_EMISSION");
    }
}
