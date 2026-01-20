using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Collectable : PlayerActivatable 
{
    ProximityTrigger[] connectedTriggers;
    


    void Awake() {
        connectedTriggers = GetComponentsInChildren<ProximityTrigger>();        
    }
    override protected void OnActivate()
    {        
        gameObject.SetActive(false);
        //foreach (var connectedTrigger in connectedTriggers) {
        //    Destroy(connectedTrigger.gameObject);
        //}
    }

}
