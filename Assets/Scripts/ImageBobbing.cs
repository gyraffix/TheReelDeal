//using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ImageBobbing : MonoBehaviour
{
    [SerializeField] private float sineWaveAmplitude = 16;
    [SerializeField] private float sineWaveSpeed = 1;

    private bool isHovering = false;

    private Animator anim;

    //[SerializeField] private float minOffsetRange = 0;
    //[Range(0, 3)]
    //[SerializeField] private float maxOffsetRange = 3;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ToggleHover()
    {
        isHovering = !isHovering;
        
        if (isHovering)
        {
            anim.SetTrigger("ImageBig");
            anim.SetTrigger("Bobbing");
        }
        else
        {
            anim.SetTrigger("ImageSmall");
            anim.SetTrigger("NoBobbing");
        }

    }

    void Update()
    {
        if (isHovering)
        {
            //Bobbing();
        }
    }

    private void Bobbing()
    {
        Vector3 rotation = transform.forward * sineWaveAmplitude * Mathf.Sin(Time.time * sineWaveSpeed);
        transform.localEulerAngles = rotation;
    }
}
