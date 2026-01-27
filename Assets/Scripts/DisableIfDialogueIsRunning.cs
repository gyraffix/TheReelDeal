using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DisableIfDialogueIsRunning : MonoBehaviour
{
    private DialogueRunner dr;
    [SerializeField] private List<GameObject> objectsToDeactivate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        dr = FindFirstObjectByType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dr.IsDialogueRunning);
        if (dr.IsDialogueRunning)
        {
            foreach (GameObject go in objectsToDeactivate)
            {
                go.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject go in objectsToDeactivate)
            {
                go.SetActive(true);
            }
        }
    }
}
