using System;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    [Header("References")]
    private RectTransform minigameRectTransform;
    public RectTransform targetRectTransform;
    public RectTransform meterRectTransform;
    // public Slider progressSlider;

    [Header("General settings")]
    public float progressIncrease = 2;
    public float progressDecrease = 1;
    private float minY;
    private float maxY;
    private float fishingProgress;

    [Header("Meter settings")]
    public KeyCode jumpInput = KeyCode.Space;
    public float gravity = 0.01f;
    public float speed = 10;
    private float meterLocation = 0;
    private float gravityMultiplier;

    [Header("Target settings")]
    public int targetHeight = 5;
    public float targetSpeed = 2;
    private bool targetGoingUp = true;
    private float targetLocation;

    void Start()
    {
        minigameRectTransform = GetComponent<RectTransform>();

        minY = -minigameRectTransform.sizeDelta.y / 2;
        maxY = minigameRectTransform.sizeDelta.y / 2;

        targetLocation = minY + targetHeight / 2;

        // progressSlider.minValue = 0;
        // progressSlider.maxValue = 100;
        // progressSlider.value = progressSlider.minValue;

        targetRectTransform.sizeDelta = new Vector2(targetRectTransform.sizeDelta.x, targetHeight);
    }

    void Update()
    {
        UpdateTarget();
        UpdatePlayer();

        if (Input.GetKey(jumpInput))
        {
            gravityMultiplier = 1;
        }

        // if (meterLocation >= targetLocation - targetHeight || meterLocation <= targetLocation + targetHeight)
        // {
        //     fishingProgress += progressIncrease;
        // }
        // else
        // {
        //     fishingProgress -= progressDecrease;
        // }
        // fishingProgress = Math.Clamp(fishingProgress, 0, 100);

        // UpdateProgress();
    }
    private void UpdateTarget()
    {
        if (targetLocation == minY + targetHeight / 2)
        {
            targetGoingUp = true;
        }
        else if (targetLocation == maxY - targetHeight / 2)
        {
            targetGoingUp = false;
        }

        targetLocation = targetGoingUp ? targetLocation + Time.deltaTime * targetSpeed : targetLocation - Time.deltaTime * targetSpeed;

        targetLocation = Math.Clamp(targetLocation, minY + targetHeight / 2, maxY - targetHeight / 2);

        targetRectTransform.localPosition = new Vector2(0, targetLocation);
    }
    private void UpdatePlayer()
    {
        gravityMultiplier = gravityMultiplier > -1 ? gravityMultiplier - Time.deltaTime * gravity : gravityMultiplier;
        gravityMultiplier = Math.Clamp(gravityMultiplier, -1, 1);

        meterLocation += Time.deltaTime * speed * gravityMultiplier;

        meterLocation = Math.Clamp(meterLocation, minY + meterRectTransform.sizeDelta.y / 2, maxY - meterRectTransform.sizeDelta.y / 2);

        meterRectTransform.localPosition = new Vector2(0, meterLocation);
    }

    // private void UpdateProgress()
    // {
    //     if (!progressSlider)
    //         return;
    //     progressSlider.value = fishingProgress;
    // }

    public float GetFishingProgress()
    {
        return fishingProgress;
    }
}
