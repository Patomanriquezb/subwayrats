using System;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public LayerMask ratLayer;
    public LayerMask obstructionLayers;
    private MeshCollider meshCollider;

    private Light light;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        light = GetComponentInChildren<Light>();
        meshCollider.includeLayers = ratLayer.value;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(CheckLineOfSight(other.transform.position)) light.color = Color.red;
            else light.color = Color.yellow;
        }
    }

    private bool CheckLineOfSight(Vector3 ratPosition)
    {
        return !Physics.Linecast(transform.parent.position, ratPosition, obstructionLayers, QueryTriggerInteraction.Ignore);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            light.color = Color.yellow;
        }
    }
}
