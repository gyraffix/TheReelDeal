using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public abstract class FishingMinigame : PlayerActivatable
{
    [Header("References")]
    [SerializeField] protected Canvas minigameCanvas;
    [SerializeField] private Transform bobberStartingPoint;
    [SerializeField] private GameObject bobberPrefab;
    [SerializeField] private StudioEventEmitter FMODThrowingBobber;
    public StudioEventEmitter FMODReelingIn;
    [SerializeField] protected StudioEventEmitter FMODVictorySound;
    [HideInInspector] public Rigidbody bobberInstance;
    [SerializeField] private GameObject wanderingFishPrefab;
    protected GameObject fishObject;
    protected Slider throwingSlider;
    protected GameObject fishCaughtText;
    protected static GameObject caughtFishSprite;
    protected Transform player;
    protected HasUsableItem hasUsableItem;
    protected DialogueRunner dialogueRunner;
    protected GameObject wanderingFish;
    

    [Header("Minigame Settings")]
    [SerializeField] protected KeyCode minigameInput = KeyCode.Space;
    [SerializeField] protected KeyCode exitMinigameInput = KeyCode.Escape;
    [SerializeField] protected int currentDifficultyIndex;
    [SerializeField] protected Difficulty[] Difficulties;
    [SerializeField] protected FishItem[] possibleFishes;
    protected float progressIncrease = 50;
    protected float progressDecrease = 10;
    protected bool active = false;
    protected MinigameState minigameState;

    [Header("Progress Settings")]
    [HideInInspector] public Vector3 fishLocation;
    protected float fishingProgress;
    protected Vector3 fishDestination;

    [Header("Bobber Settings")]
    [SerializeField] private float maxThrowingStrength = 30;
    [SerializeField] private float minThrowingStrength = 10;
    [SerializeField] private float strengthIncreaseSpeed = 10;
    private bool strengthIncreasing;
    private bool startedThrowing = false;
    [SerializeField] private float reelingSpeed;
    [SerializeField] private float maxReelingSpeed;
    [HideInInspector] public bool isReeling = false;
    [HideInInspector] public bool checkBobberDistance;

    protected virtual void Awake()
    {
        throwingSlider = minigameCanvas.transform.Find("ThrowingStrength").GetComponent<Slider>();
        fishCaughtText = minigameCanvas.transform.Find("FishCaught").gameObject;
        caughtFishSprite = minigameCanvas.transform.Find("CaughtFish").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        hasUsableItem = gameObject.GetComponent<HasUsableItem>();
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    }

    protected void Start()
    {
        throwingSlider.minValue = minThrowingStrength;
        throwingSlider.maxValue = maxThrowingStrength;
    }

    protected virtual void Update()
    {
        if (!active)
            return;
        if (fishingProgress >= 100)
            FishingSuccessful();

        if (Input.GetKeyDown(exitMinigameInput))
        {
            FirstPersonMovement.instance.active = true;
            FirstPersonLook.instance.active = true;
            Jump.instance.active = true;
            Crouch.instance.active = true;
            active = false;
            ResetMinigame();
            ProximityTriggerRoot.instance.Enabled(true);
            return;
        }

        switch (minigameState)
        {
            case MinigameState.Throwing:
                MinigameSetActive(false);
                throwingSlider.gameObject.SetActive(true);

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
                if (bobberInstance != null)
                {
                    MinigameSetActive(false);
                    throwingSlider.gameObject.SetActive(false);

                    if (Input.GetKeyDown(minigameInput))
                    {
                        isReeling = true;
                        FMODReelingIn.Play();
                    }
                    else if (Input.GetKeyUp(minigameInput))
                    {
                        isReeling = false;
                        FMODReelingIn.Stop();
                    }
                    if (isReeling)
                    {
                        bobberInstance.AddForce((player.transform.position - bobberInstance.transform.position).normalized * reelingSpeed, ForceMode.Force);
                    }
                }
                break;
        }
    }

    protected override void OnActivate()
    {
        ProximityTriggerRoot.instance.Enabled(false);
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            EndMinigame();
        }
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
        checkBobberDistance = false;
        FMODThrowingBobber.Play();
    }

    protected void UpdateFish()
    {
         wanderingFish.transform.position = fishLocation + 
            new Vector3((fishDestination.x - fishLocation.x) * (fishingProgress/100), 0, (fishDestination.z - fishLocation.z) * (fishingProgress / 100));
        Debug.Log(fishLocation + " / " + fishDestination);
    }

    protected void FishingSuccessful()
    {
        FishItem currentFish = possibleFishes[UnityEngine.Random.Range(0, possibleFishes.Length - 1)];

        caughtFishSprite.GetComponent<Image>().sprite = currentFish.fishPhoto;

        FirstPersonMovement.instance.active = true;
        Jump.instance.active = true;
        Crouch.instance.active = true;

        fishCaughtText.GetComponent<Animator>().SetTrigger("FishCaught");

        FMODVictorySound.Play();

        ResetMinigame();
        ProximityTriggerRoot.instance.Enabled(true);

        if (!Album.instance.addedFish.Contains(currentFish.name))
        {
            caughtFishSprite.GetComponent<Animator>().SetTrigger("FishCaught");
            RunDialogue(currentFish);
        }

        Album.instance.NewFish(currentFish);

        active = false;
    }

    [YarnCommand("playAnimation")]
    public static void PlayAnimation()
    {
        caughtFishSprite.GetComponent<Animator>().SetTrigger("exit");
        FirstPersonMovement.instance.active = true;
    }

    protected void RunDialogue(FishItem fish)
    {
        if (dialogueRunner != null)
        {
            if (dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.Stop();
            }
            Debug.Log(fish.name);

            dialogueRunner.StartDialogue(fish.name);
        }
        else
        {
            Debug.LogWarning("DialogueRunner component not assigned!");
        }
    }

    // This should reset all required variables (including the bobber ones) to their starting value and deactivate all the minigame's GameObjects
    protected abstract void ResetMinigame();
    protected abstract void MinigameSetActive(bool active);

    public void EndMinigame()
    {
        FirstPersonMovement.instance.active = true;
        Jump.instance.active = true;
        Crouch.instance.active = true;
        active = false;
        ResetMinigame();
        ProximityTriggerRoot.instance.Enabled(true);
    }
    private IEnumerator LateBobberDistance()
    {
        yield return new WaitForSeconds(1);
        checkBobberDistance = true;
    }

    /*
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
    */

    public void SetMinigameState(MinigameState newMinigameState)
    {
        switch (newMinigameState)
        {
            case MinigameState.Throwing:
                minigameState = MinigameState.Throwing;
                break;

            case MinigameState.Reeling:
                minigameState = MinigameState.Reeling;
                break;

            case MinigameState.Playing:
                FirstPersonMovement.instance.active = false;
                wanderingFish = Instantiate(wanderingFishPrefab, fishLocation, wanderingFishPrefab.transform.rotation);
                Debug.Log(wanderingFish.activeSelf);
                fishDestination = new Vector3(
                    player.transform.position.x + ((fishLocation.x - player.transform.position.x) * 0.2f),
                    fishLocation.y,
                    player.transform.position.z + ((fishLocation.z - player.transform.position.z) * 0.2f));
                minigameState = MinigameState.Playing;
                break;
        }
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

    [Header("Mashing Minigame settings")]
    public float requiredClicksPerSecond;
}