using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    [SerializeField] private Material higLightMaterial;
    //private Material[] originalMaterial;

    private Material originalMaterial;
   
    private void Start()
    {

        Renderer renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;

        //Renderer[] renderers = GetComponentsInChildren<Renderer>();
        //originalMaterial = new Material[renderers.Length];
        //for (int i = 0; i < renderers.Length; i++)
        //{
        //    originalMaterial[i] = renderers[i].material;
        //}
    }
    public void HighLightObject()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = higLightMaterial;

        //Renderer[] renderers = GetComponentsInChildren<Renderer>();
        //foreach (var item in renderers)
        //{
        //    item.material = higLightMaterial;
        //}
    }
    public void SetOriginalMaterialBack()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = originalMaterial;

        //Renderer[] renderers = GetComponentsInChildren<Renderer>();
        //for (int i = 0; i < renderers.Length; i++)
        //{

        //    renderers[i].material = originalMaterial[i];
        //}
    }
}
