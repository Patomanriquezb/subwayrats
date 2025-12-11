using System;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class VisionCone : MonoBehaviour
{
    public LayerMask ratLayer;
    public LayerMask obstructionLayers;
    public DialogueRunner dialogueRunner;
    
    private MeshCollider meshCollider;
    private Light light;

    public float seenTimer = 0.5f;

    private float seenTimestamp = 0;
    private bool seenFired = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        light = GetComponentInChildren<Light>();
        meshCollider.includeLayers = ratLayer.value;

        if (dialogueRunner is null) dialogueRunner = GetComponent<DialogueRunner>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("IS PLAYER");
            print($"LINE OF SIGHT: {CheckLineOfSight(other.transform.position)}");
            if (CheckLineOfSight(other.transform.position))
            {
                
                //light.color = Color.red;
                print(Time.time - seenTimestamp);
                if (Time.time - seenTimestamp >= seenTimer && !seenFired)
                {
                    seenFired = true;
                    YarnInterrupt();
                }
            }
            else
            {
                //light.color = Color.yellow;
                seenTimestamp = Time.time;
            }
        }
    }

    private bool CheckLineOfSight(Vector3 ratPosition)
    {
        RaycastHit wtf = new RaycastHit();
        if (Physics.Linecast(transform.parent.position, ratPosition, out RaycastHit lineCast,  obstructionLayers,
                QueryTriggerInteraction.Ignore))
        {
            print(lineCast.transform.name);
            return false;
        }

        print("NOTHING HIT");
        return true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            light.color = Color.yellow;
        }
    }

    private void YarnInterrupt()
    {
        print("Stopping Dialogue");
        string nodeName = dialogueRunner.Dialogue.CurrentNode;
        if (nodeName is not null && nodeName.Contains("_SeeRat")) return;
        
        string moddedNodeName = nodeName + "_SeeRat";

        print($"{nodeName} switching to {moddedNodeName}");
        
        if (dialogueRunner.Dialogue.NodeExists(moddedNodeName))
        {
            dialogueRunner.StartDialogue(moddedNodeName);
        }
        else
        {
            Debug.LogWarning($"Node {moddedNodeName} does not exist.");
        }
        
    }
}
