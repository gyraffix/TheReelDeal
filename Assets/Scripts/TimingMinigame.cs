using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingMinigame : FishingMinigame
{
    private RectTransform BackgroundRectTransform;
    private RectTransform targetsParent;
    private RectTransform meterRectTransform;
    private Slider progressSlider;

    private float minY;
    private float maxY;
    private int targetHeight = 20;
    private bool targetOverlap;
    private float meterSpeed = 50;
    private float meterPos = 0;
    private bool meterGoingUp = false;
    private float defaultProgressDecrease = 8;
    private float steadyProgressChange = 80;
    private float currentFishingProgressMax;
    private float currentFishingProgressMin;
    private bool firstPressSucces;
    private bool pressSucces;
    private bool pressed;


    [SerializeField] private List<RectTransform> targets;
    private List<RectTransform> spawnedTargets = new List<RectTransform>();
    private new void Awake()
    {
        BackgroundRectTransform = minigameCanvas.transform.Find("TimingMinigameBackground").GetComponent<RectTransform>();
        meterRectTransform = BackgroundRectTransform.transform.Find("Meter").GetComponent<RectTransform>();
        progressSlider = BackgroundRectTransform.transform.Find("Progress").GetComponent<Slider>();
        fishObject = transform.Find("Fish").gameObject;

        targetsParent = BackgroundRectTransform.transform.Find("Targets").GetComponent<RectTransform>();
        for (int i = 1;  i < targets.Count + 1; i++)
        {
            targets[i - 1] = targetsParent.transform.Find("Target" + i).GetComponent<RectTransform>();
        }

        base.Awake();
    }

    private new void Start()
    {
        base.Start();

        minY = -BackgroundRectTransform.sizeDelta.y / 2;
        maxY = BackgroundRectTransform.sizeDelta.y / 2;

        fishingProgress = 10;
        progressIncrease = 30;
        progressDecrease = 30;


        BackgroundRectTransform.gameObject.SetActive(false);

        fishObject.SetActive(false);
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

            fishingProgress = Mathf.Clamp(fishingProgress, 0, 100);

            MeterUpdate();
            UpdateProgress();
        }
    }

    private void MeterUpdate()
    {       
        if (meterGoingUp)
        {
            meterPos += Time.deltaTime * meterSpeed;
            meterRectTransform.localPosition = new Vector2(0, meterPos);
        }
        else if (!meterGoingUp)
        {
            meterPos -= Time.deltaTime * meterSpeed;
            meterRectTransform.localPosition = new Vector2(0, meterPos);
        }

        if (meterPos <= minY)
            meterGoingUp = true;
        else if (meterPos >= maxY)
            meterGoingUp = false;
    }


    protected override void OnActivate()
    {
        base.OnActivate();

        progressIncrease = Difficulties[currentDifficultyIndex].progressIncrease;
        progressDecrease = Difficulties[currentDifficultyIndex].progressDecrease;
        meterSpeed = Difficulties[currentDifficultyIndex].meterSpeed;
        targetHeight = Difficulties[currentDifficultyIndex].targetHeight;
        defaultProgressDecrease = Difficulties[currentDifficultyIndex].defaultProgressDecrease;

        Jump.instance.active = false;
        Crouch.instance.active = false;

        ResetMinigame();
        SetMinigameState(MinigameState.Throwing);       

        BackgroundRectTransform.gameObject.SetActive(true);
        MinigameSetActive(true);
        TargetUpdate();

        active = true;
    }

    private void TargetUpdate()
    {
        foreach (var target in targets)
        {
            SpawnTarget(target);
        }
    }


    private void SpawnTarget(RectTransform target)
    {
        var targetTransform = target.transform.GetComponent<RectTransform>();
        targetTransform.sizeDelta = new Vector2(targetTransform.sizeDelta.x, targetHeight);
        

        for (int count = 0; count < 1; count++)
        {
            float spawnHeight = Random.Range(minY + targetHeight / 2, maxY - targetHeight / 2);
            Vector2 spawnPos = new Vector2(0, spawnHeight);

            if (spawnedTargets.Count > 0)
            {
                for (int i = 0; i < spawnedTargets.Count; i++)
                {
                    var spawnedTarget = spawnedTargets[i];
                    float targetPos = spawnedTarget.localPosition.y;

                    Vector2 spawnBounds = new Vector2(spawnPos.y - targetHeight / 2, spawnPos.y + targetHeight / 2);
                    if ((spawnBounds.x > targetPos - targetHeight / 2 && spawnBounds.x < targetPos + targetHeight / 2) 
                        || (spawnPos.y < targetPos + targetHeight / 2 && spawnBounds.y > targetPos - targetHeight / 2))
                    {
                        targetOverlap = true;
                    }
                }
            }

            if (targetOverlap)
            {
                count--;
                targetOverlap = false;
            }
            else
            {
                targetTransform.localPosition = spawnPos;
                spawnedTargets.Add(target);
            }
        }
        //spawnedTargets.Clear();
        

    }

    void UpdateProgress()
    {
        bool increasing = false;

        if (Input.GetKeyDown(minigameInput) || Input.GetKeyDown(minigameInputMouse))
        {
            pressed = true;

            for (int i = 0; i < spawnedTargets.Count; i++)
            {
                var target = spawnedTargets[i];
                float targetPos = target.localPosition.y;
                if (meterPos > targetPos - targetHeight / 2 && meterPos < targetPos + targetHeight / 2)
                {
                    if (currentFishingProgressMax < fishingProgress)
                        currentFishingProgressMax = fishingProgress;                        
                    currentFishingProgressMax = currentFishingProgressMax + progressIncrease;
                    Debug.Log(currentFishingProgressMax);
                    firstPressSucces = true;
                    pressSucces = true;
                    SpawnTarget(target);
                }
            }
            if (!pressSucces && firstPressSucces) 
                currentFishingProgressMin = fishingProgress - progressDecrease;
        }
        if (pressed && pressSucces)
        {            
            if (fishingProgress < currentFishingProgressMax)
            {
                increasing = true;
                fishingProgress += steadyProgressChange * Time.deltaTime;           
            }
            else
            {
                pressed = false;
                pressSucces = false;
            }
        }
        else if (pressed && !pressSucces)
        {
            if (!firstPressSucces)
            {
                pressed = false;
            }
            else
            {
                if (fishingProgress > currentFishingProgressMin)
                {
                    increasing = true;
                    fishingProgress -= steadyProgressChange * Time.deltaTime;
                }
                else
                    pressed = false;            
            }
        }       
        else if (!pressed && !increasing && firstPressSucces)
        {
            //Debug.Log("Decreasing");
            fishingProgress -= defaultProgressDecrease * Time.deltaTime;
        }

        UpdateFish();
        progressSlider.value = fishingProgress;
        
        if (progressSlider.value > 99)
        {
            Debug.Log("Victory");
            FishingSuccessful();
        }

        //foreach (var target in spawnedTargets)
        //{
        //    float targetPos = target.localPosition.y;
        //    if ((meterPos > targetPos - targetHeight / 2 && meterPos < targetPos + targetHeight / 2) && Input.GetKeyDown(minigameInput))
        //    {
        //        fishingProgress += progressIncrease * Time.deltaTime;
        //    }
        //    else
        //        fishingProgress -= progressDecrease * Time.deltaTime;

        //    progressSlider.value = fishingProgress;
        //}
        //Debug.Log(progressSlider.value);
    }

    protected override void ResetMinigame()
    {
        firstPressSucces = false;
        fishingProgress = 10;
        meterPos = 0;
        currentFishingProgressMax = 0;
        currentFishingProgressMin = 0;
        if (spawnedTargets.Count > 0)         
            spawnedTargets.Clear();

        checkBobberDistance = false;

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
        }
        else
        {
            BackgroundRectTransform.gameObject.SetActive(false);
        }
    }
}
