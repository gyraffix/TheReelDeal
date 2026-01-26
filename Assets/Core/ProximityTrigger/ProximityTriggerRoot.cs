using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityTriggerRoot : MonoBehaviour
{
    public static ProximityTriggerRoot instance;

    private void Awake()
    {
        instance = this;
    }

    public void Enabled(bool active)
    {
        if (!active)        
            gameObject.SetActive(false);        
        else 
            gameObject.SetActive(true);
    }

}

