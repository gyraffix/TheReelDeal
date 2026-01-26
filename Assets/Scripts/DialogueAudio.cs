using FMODUnity;
using System.Collections;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class DialogueAudio : MonoBehaviour
{
    public StudioEventEmitter FMODDialogueSound;
    private string lastText = "";
    private string currentText = "";
    private int charsPerSecond = 60;

    private TMP_Text text;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<TMP_Text>();
    }

    [YarnCommand]
    public void StopSound()
    {
        FMODDialogueSound.Stop();
    }

    private void Update()
    {
        currentText = text.text;



        if ((lastText != null && currentText != null) && !lastText.Equals(currentText))
        {
            StopAllCoroutines();
            if (FMODDialogueSound.IsPlaying())
            {
                FMODDialogueSound.Stop();
            }

            float letterAmount = currentText.Length;

            StartCoroutine(PlaySound((float)letterAmount / charsPerSecond));
        }

        lastText = currentText;
    }

    IEnumerator PlaySound(float time)
    {
        FMODDialogueSound.SetParameter("LoopDialogue", 1);
        FMODDialogueSound.Play();
        Debug.Log("Loop Sound here");
        yield return new WaitForSeconds(time);
        Debug.Log("Stop Sound here");
        FMODDialogueSound.SetParameter("LoopDialogue", 0);
    }

}
