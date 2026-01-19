using FMODUnity;
using UnityEngine;
using Yarn.Unity;

public static class DialogueAudio
{
    public static StudioEventEmitter FMODDialogueSound;
    [YarnCommand("FindEventEmitter")]
    public static void FindEventEmitter(string objectName)
    {
        FMODDialogueSound = GameObject.Find(objectName).GetComponent<StudioEventEmitter>();
    }

    [YarnCommand("StartDialogueSound")]
    public static void StartDialogueSound()
    {
        FMODDialogueSound.Play();
    }

    [YarnCommand("EndDialogueSound")]
    public static void EndDialogueSound()
    {
        FMODDialogueSound.Stop();
    }
}
