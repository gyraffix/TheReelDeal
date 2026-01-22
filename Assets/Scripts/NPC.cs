using UnityEngine;
using Yarn.Unity;

public class NPC : PlayerActivatable
{
    [Header("Set this to true if the NPC has a quest")]
    [SerializeField] private bool isQuestNPC;

    [Header("Non-Quest Settings")]
    [SerializeField] private string Dialogue;


    [Header("Quest Settings")]  
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

    private void Update()
    {
        transform.LookAt(new Vector3
            (
            FirstPersonMovement.instance.gameObject.transform.position.x,
            transform.position.y,
            FirstPersonMovement.instance.gameObject.transform.position.z)
            );
    }

    protected override void OnActivate()
    {
        Debug.Log("activated NPC");

        FirstPersonLook.instance.active = false;
        FirstPersonMovement.instance.active = false;
        player.transform.LookAt(new Vector3
            (
            transform.position.x,
            FirstPersonMovement.instance.gameObject.transform.position.y, 
            transform.position.z)
            );

        if (isQuestNPC)
        {
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

        else
        {
            RunDialogue(Dialogue);
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
