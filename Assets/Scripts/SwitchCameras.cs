using UnityEngine;
using Yarn.Unity;

public static class SwitchCameras
{
    [YarnCommand("NextCamera")]
    public static void NextCamera()
    {
        Animator cameraAnimator = GameObject.Find("CutsceneCam").GetComponent<Animator>();
        cameraAnimator.SetTrigger("NextCutScene");
    }
}
