using UnityEngine;
using Yarn.Unity;

public class NPC : PlayerActivatable
{
    [SerializeField] private int fishNeeded;
    [SerializeField] private Collider barrier;
    [SerializeField] private string completedDialogue;
    [SerializeField] private string incompleteDialogue;

    private GameObject player;
    private DialogueRunner dr;

    private void Awake()
    {
        dr = FindFirstObjectByType<DialogueRunner>();
        player = FindFirstObjectByType<FirstPersonMovement>().gameObject;
    }

    

    protected override void OnActivate()
    {

        Debug.Log("activated NPC");

        FirstPersonLook.instance.active = false;
        FirstPersonMovement.instance.active = false;
        player.transform.LookAt(transform.position);



        if (fishNeeded <= Album.instance.addedFish.Count)
        {
            RunDialogue(completedDialogue);
            barrier.isTrigger = true;
        }
        else
        {
            RunDialogue(incompleteDialogue);
        }
    }



    private void RunDialogue(string tag)
    {
        if (dr != null)
        {
            if (dr.IsDialogueRunning)
            {
                dr.Stop();
            }

            dr.StartDialogue(tag);
        }
        else
        {
            Debug.LogWarning("DialogueRunner component not assigned!");
        }
    }

}
