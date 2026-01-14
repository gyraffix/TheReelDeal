//using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ImageBobbing : MonoBehaviour
{
    [SerializeField] private float sineWaveAmplitude = 16;
    [SerializeField] private float sineWaveSpeed = 1;

    //[SerializeField] private float minOffsetRange = 0;
    //[Range(0, 3)]
    //[SerializeField] private float maxOffsetRange = 3;

    
    private bool hovering = false;
    public void ToggleHover()
    {
        hovering = !hovering;
    }

    void Update()
    {
        if (hovering)
        {
            //if (Time.time > offset) 
                Bobbing();
        }
    }

    private void Bobbing()
    {
        Vector3 rotation = transform.forward * sineWaveAmplitude * Mathf.Sin(Time.time * sineWaveSpeed);
        transform.localEulerAngles = rotation;
    }
}
