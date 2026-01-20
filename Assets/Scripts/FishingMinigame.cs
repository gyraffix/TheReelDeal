using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingMinigame : PlayerActivatable
{
    [Header("References")]
    [SerializeField] private Canvas minigameCanvas;
    [SerializeField] private Transform bobberStartingPoint;
    [SerializeField] private GameObject bobberPrefab;
    [HideInInspector] public Rigidbody bobberInstance;
    private RectTransform BackgroundRectTransform;
    private RectTransform targetRectTransform;
    private RectTransform meterRectTransform;
    private Slider progressSlider;
    private Slider throwingSlider;
    private GameObject FishCaughtText;
    private GameObject CaughtFishSprite;
    private Transform player;
    private HasUsableItem hasUsableItem;

    [Header("Minigame Settings")]
    [SerializeField] private KeyCode minigameInput = KeyCode.Space;
    [SerializeField] private KeyCode exitMinigameInput = KeyCode.Escape;
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
    private MinigameState minigameState;

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

    [Header("Progress Settings")]
    [SerializeField] private float fishHeight;
    [SerializeField] private float fishEndZ = 4f;
    [SerializeField] private float fishStartZ = 7f;
    [SerializeField] private float sineWaveAmplitude = 16;
    [SerializeField] private float sineWaveSpeed = 1;
    private GameObject fishObject;
    private float fishingProgress;

    [Header("Bobber Settings")]
    [SerializeField] private float minThrowingStrength = 100;
    [SerializeField] private float maxThrowingStrength = 50;
    [SerializeField] private float strengthIncreaseSpeed = 10;
    private bool strengthIncreasing;
    private bool startedThrowing = false;
    [SerializeField] private float reelingSpeed;
    [SerializeField] private float maxReelingSpeed;
    [SerializeField] private float minimumDistanceToPlayer;
    private bool checkBobberDistance;

    void Awake()
    {
        BackgroundRectTransform = minigameCanvas.transform.Find("MinigameBackground").GetComponent<RectTransform>();
        targetRectTransform = BackgroundRectTransform.transform.Find("Target").GetComponent<RectTransform>();
        meterRectTransform = BackgroundRectTransform.transform.Find("Meter").GetComponent<RectTransform>();
        progressSlider = BackgroundRectTransform.transform.Find("Progress").GetComponent<Slider>();
        throwingSlider = minigameCanvas.transform.Find("ThrowingStrength").GetComponent<Slider>();
        FishCaughtText = minigameCanvas.transform.Find("FishCaught").gameObject;
        CaughtFishSprite = minigameCanvas.transform.Find("CaughtFish").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        fishObject = transform.Find("Fish").gameObject;
        hasUsableItem = gameObject.GetComponent<HasUsableItem>();
    }
    void Start()
    {
        minY = -BackgroundRectTransform.sizeDelta.y / 2;
        maxY = BackgroundRectTransform.sizeDelta.y / 2;

        progressSlider.minValue = 0;
        progressSlider.maxValue = 100;

        throwingSlider.minValue = minThrowingStrength;
        throwingSlider.maxValue = maxThrowingStrength;

        BackgroundRectTransform.gameObject.SetActive(false);
        fishObject.SetActive(false);
    }

    void Update()
    {
        if (!active)
            return;
        if (fishingProgress >= 100)
            FishingSuccessful();

        if (Input.GetKeyDown(exitMinigameInput))
        {
            FirstPersonLook.instance.active = true;
            FirstPersonMovement.instance.active = true;
            Jump.instance.active = true;
            Crouch.instance.active = true;
            active = false;
            ResetMinigame();
            return;
        }

        switch (minigameState)
        {
            case MinigameState.Throwing:
                if (Input.GetKey(minigameInput))
                {
                    startedThrowing = true;
                    UpdateStrength();
                }
                else if (startedThrowing == true)
                {
                    startedThrowing = false;
                    ThrowBobber();

                    StartCoroutine(LateBobberDistance());
                }
                break;

            case MinigameState.Reeling:
                if ((player.transform.position - bobberInstance.transform.position).magnitude > minimumDistanceToPlayer)
                {
                    if (Input.GetKey(minigameInput))
                    {
                        bobberInstance.AddForce((player.transform.position - bobberInstance.transform.position).normalized * reelingSpeed, ForceMode.Force);
                    }
                }
                else if (checkBobberDistance)
                {
                    Destroy(bobberInstance.gameObject);
                    bobberInstance = null;
                    checkBobberDistance = false;
                    SetMinigameState(MinigameState.Throwing);
                }
                break;

            case MinigameState.Playing:
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
                break;
        }
    }

    protected override void OnActivate()
    {
        if (hasUsableItem.CheckForItem())
            currentDifficultyIndex = 2;
        else
            currentDifficultyIndex = 0;

        // FirstPersonLook.instance.active = false;
        FirstPersonMovement.instance.active = false;
        Jump.instance.active = false;
        Crouch.instance.active = false;

        ResetMinigame();
        SetMinigameState(MinigameState.Throwing);

        fishObject.SetActive(true);

        progressIncrease = Difficulties[currentDifficultyIndex].progressIncrease;
        progressDecrease = Difficulties[currentDifficultyIndex].progressDecrease;
        directionChangeSpeed = Difficulties[currentDifficultyIndex].directionChangeSpeed;
        meterSpeed = Difficulties[currentDifficultyIndex].meterSpeed;
        targetHeight = Difficulties[currentDifficultyIndex].targetHeight;
        targetSpeed = Difficulties[currentDifficultyIndex].targetSpeed;

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
        fishObject.transform.localPosition = new Vector3(
            (sineWaveAmplitude - sineWaveAmplitude * (fishingProgress / 100)) * Mathf.Sin(Time.time * sineWaveSpeed),
            fishHeight,
            fishStartZ - ((fishStartZ - fishEndZ) * fishingProgress / 100));

        progressSlider.value = fishingProgress;
    }

    private void UpdateStrength()
    {
        float newThrowingStrength = throwingSlider.value;

        if (newThrowingStrength <= minThrowingStrength)
        {
            strengthIncreasing = true;
        }
        else if (newThrowingStrength >= maxThrowingStrength)
        {
            strengthIncreasing = false;
        }

        newThrowingStrength = strengthIncreasing ? newThrowingStrength + Time.deltaTime * strengthIncreaseSpeed : newThrowingStrength - Time.deltaTime * strengthIncreaseSpeed;

        throwingSlider.value = newThrowingStrength;
    }

    private void ThrowBobber()
    {
        bobberInstance = Instantiate(bobberPrefab.gameObject, bobberStartingPoint.position, bobberStartingPoint.rotation).GetComponent<Rigidbody>();
        bobberInstance.AddForce(
            Quaternion.Euler(
                player.Find("First Person Camera").transform.rotation.eulerAngles.x,
                player.rotation.eulerAngles.y,
                0) *
            new Vector3(0, 0, throwingSlider.value),
            ForceMode.VelocityChange);
        bobberInstance.GetComponent<BobberScript>().fishingMinigame = this;
        bobberInstance.GetComponent<BobberScript>().maxSpeed = maxReelingSpeed;
        throwingSlider.value = 0;
        SetMinigameState(MinigameState.Reeling);
    }

    public void SetMinigameState(MinigameState newMinigameState)
    {
        switch (newMinigameState)
        {
            case MinigameState.Throwing:
                BackgroundRectTransform.gameObject.SetActive(false);
                throwingSlider.gameObject.SetActive(true);
                minigameState = MinigameState.Throwing;
                break;

            case MinigameState.Reeling:
                BackgroundRectTransform.gameObject.SetActive(false);
                throwingSlider.gameObject.SetActive(false);
                minigameState = MinigameState.Reeling;
                break;

            case MinigameState.Playing:
                BackgroundRectTransform.gameObject.SetActive(true);
                throwingSlider.gameObject.SetActive(false);
                minigameState = MinigameState.Playing;
                break;

            default:
                BackgroundRectTransform.gameObject.SetActive(false);
                throwingSlider.gameObject.SetActive(true);
                minigameState = MinigameState.Throwing;
                break;
        }
    }

    private void FishingSuccessful()
    {
        FishItem currentFish = possibleFishes[UnityEngine.Random.Range(0, possibleFishes.Length - 1)];

        Album.instance.NewFish(currentFish);

        CaughtFishSprite.GetComponent<Image>().sprite = currentFish.fishPhoto;

        FirstPersonLook.instance.active = true;
        FirstPersonMovement.instance.active = true;
        Jump.instance.active = true;
        Crouch.instance.active = true;

        FishCaughtText.GetComponent<Animator>().SetTrigger("FishCaught");
        CaughtFishSprite.GetComponent<Animator>().SetTrigger("FishCaught");
        

        BackgroundRectTransform.gameObject.SetActive(false);
        fishObject.SetActive(false);

        

        active = false;
    }

    private void ResetMinigame()
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
        BackgroundRectTransform.gameObject.SetActive(false);
        throwingSlider.gameObject.SetActive(false);
    }

    private IEnumerator LateBobberDistance()
    {
        yield return new WaitForSeconds(1);
        checkBobberDistance = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(new Vector3(0, fishHeight, fishStartZ),
            new Vector3(0, fishHeight, fishEndZ));

        Gizmos.DrawLine(new Vector3(-sineWaveAmplitude, fishHeight, fishStartZ),
            new Vector3(+sineWaveAmplitude, fishHeight, fishStartZ));

        Gizmos.DrawLine(new Vector3(-sineWaveAmplitude, fishHeight, fishStartZ),
            new Vector3(0, fishHeight, fishEndZ));

        Gizmos.DrawLine(new Vector3(sineWaveAmplitude, fishHeight, fishStartZ),
            new Vector3(0, fishHeight, fishEndZ));
    }

    public enum MinigameState
    {
        Throwing,
        Reeling,
        Playing
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