//using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ImageBobbing : MonoBehaviour
{
    [SerializeField] private float sineWaveAmplitude = 16;
    [SerializeField] private float sineWaveSpeed = 1;

    [SerializeField] private float minOffsetRange = 0;
    [Range(0, 3)]
    [SerializeField] private float maxOffsetRange = 3;
    [SerializeField] private float offset;

    private void Awake()
    {
        offset = Random.Range(minOffsetRange, maxOffsetRange);
    }

    void Update()
    {
        //if (Time.time > offset) 
            Bobbing();
    }

    private void Bobbing()
    {
        Vector3 rotation = transform.forward * sineWaveAmplitude * Mathf.Sin(Time.time * sineWaveSpeed);
        transform.localEulerAngles = rotation;
    }
}
