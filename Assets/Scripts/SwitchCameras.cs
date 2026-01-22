using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Yarn.Unity;

public class SwitchCameras : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras;
    private int currentCam = 0;



    [YarnCommand]
    public void NextCamera()
    {
        cameras[currentCam].gameObject.SetActive(false);
        currentCam++;
        if (currentCam != cameras.Count) 
        {
            cameras[currentCam].gameObject.SetActive(true);
        }      
    }

}
