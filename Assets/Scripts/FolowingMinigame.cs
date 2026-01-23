using System;
using UnityEngine;
using UnityEngine.UI;

public class FolowingMinigame : FishingMinigame
{
    [Header("Refferences")]
    private RectTransform BackgroundRectTransform;
    private RectTransform targetRectTransform;
    private RectTransform meterRectTransform;
    private Slider progressSlider;

    [Header("Minigame Settings")]
    private float currentMinY;
    private float minY;
    private float currentMaxY;
    private float maxY;

    [Header("Meter Settings")]
    private float directionChangeSpeed = 20;
    private float meterSpeed = 150;
    private float meterLocation = 0;
    private float direction = -1;

    [Header("Target Settings")]
    [SerializeField] private float minimumTravelDistance = 30;
    private int targetHeight = 20;
    private float targetSpeed = 100;
    private bool targetGoingUp = true;
    private float targetLocation;

    private new void Awake()
    {
        base.Awake();

        BackgroundRectTransform = minigameCanvas.transform.Find("MinigameBackground").GetComponent<RectTransform>();
        targetRectTransform = BackgroundRectTransform.transform.Find("Target").GetComponent<RectTransform>();
        meterRectTransform = BackgroundRectTransform.transform.Find("Meter").GetComponent<RectTransform>();
        progressSlider = BackgroundRectTransform.transform.Find("Progress").GetComponent<Slider>();
        //fishObject = transform.Find("Fish").gameObject;
    }

    private new void Start()
    {
        base.Start();

        minY = -BackgroundRectTransform.sizeDelta.y / 2;
        maxY = BackgroundRectTransform.sizeDelta.y / 2;

        progressSlider.minValue = 0;
        progressSlider.maxValue = 100;
        BackgroundRectTransform.gameObject.SetActive(false);

        targetRectTransform.sizeDelta = new Vector2(targetRectTransform.sizeDelta.x, targetHeight);

        ///fishObject.SetActive(false);
    }

    private new void Update()
    {
        base.Update();
        if (!active)
            return;
        if (minigameState == MinigameState.Playing)
        {
            MinigameSetActive(true);
            throwingSlider.gameObject.SetActive(false);

            if (Input.GetKey(minigameInput))
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
            UpdateMeterLocation();

            fishingProgress = Math.Clamp(fishingProgress, 0, 100);

            UpdateProgress();
            UpdateFish();
        }

    }

    protected override void OnActivate()
    {
        if (hasUsableItem.CheckForItem())
        {
            Debug.Log("2");
            currentDifficultyIndex = 2;
        }
        else
            currentDifficultyIndex = 0;

        Jump.instance.active = false;
        Crouch.instance.active = false;

        ResetMinigame();
        SetMinigameState(MinigameState.Throwing);

        progressIncrease = Difficulties[currentDifficultyIndex].progressIncrease;
        progressDecrease = Difficulties[currentDifficultyIndex].progressDecrease;
        directionChangeSpeed = Difficulties[currentDifficultyIndex].directionChangeSpeed;
        meterSpeed = Difficulties[currentDifficultyIndex].meterSpeed;
        targetHeight = Difficulties[currentDifficultyIndex].targetHeight;
        targetSpeed = Difficulties[currentDifficultyIndex].targetSpeed;

        active = true;
    }

    private void UpdateTargetLocation()
    {
        if (targetLocation <= currentMinY + targetHeight / 2)
        {
            targetGoingUp = true;
            if (Difficulties[currentDifficultyIndex].randomizeMovement)
            {
                currentMaxY = UnityEngine.Random.Range(targetLocation + minimumTravelDistance, maxY);
            }
        }
        else if (targetLocation >= currentMaxY - targetHeight / 2)
        {
            targetGoingUp = false;
            if (Difficulties[currentDifficultyIndex].randomizeMovement)
            {
                currentMinY = UnityEngine.Random.Range(minY, targetLocation - minimumTravelDistance);
            }
        }

        targetLocation = targetGoingUp ? targetLocation + Time.deltaTime * targetSpeed : targetLocation - Time.deltaTime * targetSpeed;

        targetLocation = Math.Clamp(targetLocation, currentMinY + targetHeight / 2, currentMaxY - targetHeight / 2);

        targetRectTransform.localPosition = new Vector2(0, targetLocation);
    }

    private void UpdateMeterLocation()
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

    protected override void ResetMinigame()
    {
        fishingProgress = 0;

        currentMinY = minY;
        currentMaxY = maxY;

        meterLocation = currentMinY + meterRectTransform.sizeDelta.y / 2;
        targetLocation = currentMinY + targetHeight / 2;

        throwingSlider.value = 0;
        if (bobberInstance != null)
        {
            Destroy(bobberInstance.gameObject);
            bobberInstance = null;
        }

        FMODReelingIn.Stop();

        BackgroundRectTransform.gameObject.SetActive(false);
        throwingSlider.gameObject.SetActive(false);
    }

    protected override void MinigameSetActive(bool active)
    {
        if (active)
        {
            BackgroundRectTransform.gameObject.SetActive(true);

            // fishObject.SetActive(true);
            // fishObject.transform.position = new Vector3(fishLocation.x, fishHeight, fishLocation.z);
            // fishStartZ = (fishLocation - transform.position).z;
            // fishEndPos = new Vector3(player.position.x - transform.position.x, fishHeight, player.position.z - transform.position.z);
        }
        else
        {
            BackgroundRectTransform.gameObject.SetActive(false);
        }
    }
}

