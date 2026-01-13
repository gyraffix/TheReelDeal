using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [Header("References")]
    private Slider slider;

    [Header("General settings")]
    private int min = 0;
    private int max = 100;

    [Header("Player settings")]
    [Range(-1, 1)] private float playerMultiplier;
    public float gravityMultiplier;
    private float playerLocation = 0;
    public float gravity = 1;

    [Header("Target settings")]
    public float targetSpeed = 2;
    private bool targetGoingUp = true;
    public float targetRange = 5;
    private float targetLocation = 5;



    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        UpdateTarget();
    }
    private void UpdateTarget()
    {
        if (targetLocation == min + targetRange)
        {
            targetGoingUp = true;
        }
        else if (targetLocation == max - targetRange)
        {
            targetGoingUp = false;
        }

        targetLocation = targetGoingUp ? targetLocation + Time.deltaTime * targetSpeed : targetLocation - Time.deltaTime * targetSpeed;

        Math.Clamp(targetLocation, min + targetRange, max - targetRange);
    }
    private void UpdatePlayer()
    {
        playerMultiplier = gravity > -1 ? gravity - Time.deltaTime * gravityMultiplier : gravity;
        Math.Clamp(gravity, min, max);

        playerLocation -= Time.deltaTime * playerMultiplier;

        Math.Clamp(playerLocation, min, max);
    }
}
