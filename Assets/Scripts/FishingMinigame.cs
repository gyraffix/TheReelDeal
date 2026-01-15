using System;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : PlayerActivatable
{
    [Header("References")]
    [SerializeField] private Canvas minigameCanvas;
    private RectTransform BackgroundRectTransform;
    private RectTransform targetRectTransform;
    private RectTransform meterRectTransform;
    private Slider progressSlider;
    private GameObject FishCaughtText;
    private HasUsableItem hasUsableItem;

    [Header("Minigame settings")]
    [SerializeField] private KeyCode jumpInput = KeyCode.Space;
    [SerializeField] private int currentDifficultyIndex;
    [SerializeField] private Difficulty[] Difficulties;
    [SerializeField] private FishItem[] possibleFishes;
    private float progressIncrease = 50;
    private float progressDecrease = 10;
    private float currentMinY;
    private float minY;
    private float currentMaxY;
    private float maxY;
    private bool active = false;

    [Header("Meter settings")]
    private float directionChangeSpeed = 20;
    private float meterSpeed = 150;
    private float meterLocation = 0;
    private float direction = -1;

    [Header("Target settings")]
    [SerializeField] private float minimumTravelDistance = 30;
    private int targetHeight = 20;
    private float targetSpeed = 100;
    private bool targetGoingUp = true;
    private float targetLocation;

    [Header("Progress Settings")]
    [SerializeField] private float fishHeight;
    [SerializeField] private float fishEndZ = 4f;
    [SerializeField] private float fishStartZ = 7f;
    private GameObject fishObject;
    private float fishingProgress;

    void Awake()
    {
        BackgroundRectTransform = minigameCanvas.transform.Find("Background").GetComponent<RectTransform>();
        targetRectTransform = BackgroundRectTransform.transform.Find("Target").GetComponent<RectTransform>();
        meterRectTransform = BackgroundRectTransform.transform.Find("Meter").GetComponent<RectTransform>();
        progressSlider = BackgroundRectTransform.transform.Find("Progress").GetComponent<Slider>();
        FishCaughtText = minigameCanvas.transform.Find("FishCaught").gameObject;
        fishObject = transform.Find("Fish").gameObject;

        hasUsableItem = gameObject.GetComponent<HasUsableItem>();
    }
    void Start()
    {
        minY = -BackgroundRectTransform.sizeDelta.y / 2;
        maxY = BackgroundRectTransform.sizeDelta.y / 2;

        currentMinY = minY;
        currentMaxY = maxY;

        progressSlider.minValue = 0;
        progressSlider.maxValue = 100;

        BackgroundRectTransform.gameObject.SetActive(false);
        fishObject.SetActive(false);
    }

    void Update()
    {
        if (!active)
            return;
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
        if (hasUsableItem.CheckForItem())
            currentDifficultyIndex = 2;
        else
            currentDifficultyIndex = 0;
        Debug.Log(currentDifficultyIndex);

            FirstPersonLook.instance.active = false;
        FirstPersonMovement.instance.active = false;
        Jump.instance.active = false;
        Crouch.instance.active = false;

        fishingProgress = 0;
        BackgroundRectTransform.gameObject.SetActive(true);
        fishObject.SetActive(true);

        progressIncrease = Difficulties[currentDifficultyIndex].progressIncrease;
        progressDecrease = Difficulties[currentDifficultyIndex].progressDecrease;
        directionChangeSpeed = Difficulties[currentDifficultyIndex].directionChangeSpeed;
        meterSpeed = Difficulties[currentDifficultyIndex].meterSpeed;
        targetHeight = Difficulties[currentDifficultyIndex].targetHeight;
        targetSpeed = Difficulties[currentDifficultyIndex].targetSpeed;

        targetLocation = currentMinY + targetHeight / 2;

        targetRectTransform.sizeDelta = new Vector2(targetRectTransform.sizeDelta.x, targetHeight);

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
        fishObject.transform.position = new Vector3(
            fishObject.transform.position.x,
            fishHeight,
            fishStartZ - ((fishStartZ - fishEndZ) * fishingProgress / 100));
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

        FishCaughtText.GetComponent<Animator>().SetTrigger("FishCaught");

        BackgroundRectTransform.gameObject.SetActive(false);
        fishObject.SetActive(false);
        active = false;
    }

    public float GetFishingProgress()
    {
        return fishingProgress;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(new Vector3(transform.position.x, fishHeight, fishStartZ), new Vector3(transform.position.x, fishHeight, fishEndZ));
    }
}

[Serializable]
public class Difficulty
{
    [Header("General settings")]
    public bool randomizeMovement;
    public float progressIncrease = 50;
    public float progressDecrease = 10;

    [Header("Meter settings")]
    public float directionChangeSpeed = 10;
    public float meterSpeed = 150;

    [Header("Target settings")]
    public int targetHeight = 20;
    public float targetSpeed = 100;
}