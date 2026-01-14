using System;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : PlayerActivatable
{
    [Header("References")]
    public Canvas minigameCanvas;
    private RectTransform BackgroundRectTransform;
    private RectTransform targetRectTransform;
    private RectTransform meterRectTransform;
    private Slider progressSlider;

    [Header("Minigame settings")]
    public KeyCode jumpInput = KeyCode.Space;
    public int currentDifficultyIndex;
    public Difficulty[] Difficulties;
    public FishItem[] possibleFishes;
    private float progressIncrease = 50;
    private float progressDecrease = 10;
    private float minY;
    private float maxY;
    private float fishingProgress;

    [Header("Meter settings")]
    private float directionChangeSpeed = 20;
    private float meterSpeed = 150;
    private float meterLocation = 0;
    private float direction = -1;

    [Header("Target settings")]
    private int targetHeight = 20;
    private float targetSpeed = 100;
    private bool targetGoingUp = true;
    private float targetLocation;

    private bool active = false;

    void Awake()
    {
        BackgroundRectTransform = minigameCanvas.transform.Find("Background").GetComponent<RectTransform>();
        targetRectTransform = BackgroundRectTransform.transform.Find("Target").GetComponent<RectTransform>();
        meterRectTransform = BackgroundRectTransform.transform.Find("Meter").GetComponent<RectTransform>();
        progressSlider = BackgroundRectTransform.transform.Find("Progress").GetComponent<Slider>();
    }
    void Start()
    {
        minY = -BackgroundRectTransform.sizeDelta.y / 2;
        maxY = BackgroundRectTransform.sizeDelta.y / 2;

        progressSlider.minValue = 0;
        progressSlider.maxValue = 100;


        BackgroundRectTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!active)
            return;
        Debug.Log(fishingProgress);
        if (fishingProgress >= 100)
            FishingSuccessful();

        if (Input.GetKey(jumpInput))
        {
            if (direction < 1)
                direction += Time.deltaTime * directionChangeSpeed;
        }
        else
        {
            if (direction > -1)
                direction -= Time.deltaTime * directionChangeSpeed;
        }

        UpdateTargetLocation();
        UpdatePlayerLocation();

        fishingProgress = Math.Clamp(fishingProgress, 0, 100);

        UpdateProgress();
    }

    protected override void OnActivate()
    {
        FirstPersonLook.instance.active = false;
        FirstPersonMovement.instance.active = false;
        Jump.instance.active = false;
        Crouch.instance.active = false;

        progressSlider.value = 0;
        BackgroundRectTransform.gameObject.SetActive(true);

        progressIncrease = Difficulties[currentDifficultyIndex].progressIncrease;
        progressDecrease = Difficulties[currentDifficultyIndex].progressDecrease;
        directionChangeSpeed = Difficulties[currentDifficultyIndex].directionChangeSpeed;
        meterSpeed = Difficulties[currentDifficultyIndex].meterSpeed;
        targetHeight = Difficulties[currentDifficultyIndex].targetHeight;
        targetSpeed = Difficulties[currentDifficultyIndex].targetSpeed;

        targetLocation = minY + targetHeight / 2;

        targetRectTransform.sizeDelta = new Vector2(targetRectTransform.sizeDelta.x, targetHeight);
        Debug.Log(fishingProgress);
        active = true;
    }

    private void UpdateTargetLocation()
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

    private void UpdatePlayerLocation()
    {
        meterLocation += Time.deltaTime * meterSpeed * Math.Clamp(direction, -1, 1);

        meterLocation = Math.Clamp(meterLocation, minY + meterRectTransform.sizeDelta.y / 2, maxY - meterRectTransform.sizeDelta.y / 2);

        meterRectTransform.localPosition = new Vector2(0, meterLocation);
    }

    private void UpdateProgress()
    {
        if (meterLocation <= targetLocation - targetHeight / 2 || meterLocation >= targetLocation + targetHeight / 2)
        {
            fishingProgress -= progressDecrease * Time.deltaTime;
        }
        else
        {
            fishingProgress += progressIncrease * Time.deltaTime;
        }

        progressSlider.value = fishingProgress;
    }

    private void FishingSuccessful()
    {
        FishItem currentFish = possibleFishes[UnityEngine.Random.Range(0, possibleFishes.Length - 1)];

        Album.instance.NewFish(currentFish);
        Debug.Log(currentFish.name);

        FirstPersonLook.instance.active = true;
        FirstPersonMovement.instance.active = true;
        Jump.instance.active = true;
        Crouch.instance.active = true;

        BackgroundRectTransform.gameObject.SetActive(false);
        active = false;
    }

    public float GetFishingProgress()
    {
        return fishingProgress;
    }
}

[Serializable]
public class Difficulty
{
    [Header("General settings")]
    public float progressIncrease = 50;
    public float progressDecrease = 10;

    [Header("Meter settings")]
    public float directionChangeSpeed = 10;
    public float meterSpeed = 150;

    [Header("Target settings")]
    public int targetHeight = 20;
    public float targetSpeed = 100;
}