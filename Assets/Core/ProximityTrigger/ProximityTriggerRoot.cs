using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ProximityTriggerRoot : MonoBehaviour
{
    public static ProximityTriggerRoot instance;
    private static List<GameObject> proximityTriggers = new List<GameObject>();


    private void Awake()
    {
        instance = this;        
    }

    [YarnCommand("enable_triggers")]
    public static void DissableTrigger()
    {
        Enabled(true);
    }

    public static void Enabled(bool active)
    {
        Transform root = FindFirstObjectByType<ProximityTriggerRoot>().transform;

        for (int i = 0; i < root.transform.childCount; i++)
        {
            proximityTriggers.Add(root.GetChild(i).gameObject);
        }

        Debug.Log(root);
        if (active)
        {
            foreach (var trigger in proximityTriggers)
            {
                trigger.SetActive(true);
            }
            proximityTriggers.Clear();
        }
        else
        {
            foreach (var trigger in proximityTriggers)
            {
                trigger.SetActive(false);
            }        
        }
    }

}

