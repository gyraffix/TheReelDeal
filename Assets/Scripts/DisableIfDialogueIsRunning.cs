using UnityEngine;
using Yarn.Unity;

public class DisableIfDialogueIsRunning : MonoBehaviour
{
    private DialogueRunner dr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        dr = FindFirstObjectByType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dr.IsDialogueRunning)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
