using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class DialogueScript : MonoBehaviour
{
    public static string lastTriggeredNode = "";
    public string yarnNode;
    public DialogueRunner dialogueRunner;

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (dialogueRunner.IsDialogueRunning && lastTriggeredNode == yarnNode) return;
            dialogueRunner.StartDialogue(yarnNode);
            lastTriggeredNode = yarnNode;
        }
    }

}