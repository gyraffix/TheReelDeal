using System;
using UnityEngine;
using Yarn.Unity;

public static class SwitchCameras
{
    [YarnCommand("NextCamera")]
    public static void NextCamera()
    {
        try
        {
            Animator cameraAnimator = GameObject.Find("CutsceneCam").GetComponent<Animator>();
            cameraAnimator.SetTrigger("NextCutScene");
        }
        catch
        {
            Console.Error.WriteLine("Couldn't find CutsceneCam");
        }
    }
}
