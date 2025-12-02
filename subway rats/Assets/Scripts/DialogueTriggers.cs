using UnityEngine;
using Yarn.Unity;

public class DialogueScript : MonoBehaviour
{
    public string yarnNode;
    public DialogueRunner dialogueRunner;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueRunner.StartDialogue(yarnNode);
        }
    }

}