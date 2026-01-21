using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimingMinigame : FishingMinigame
{
    private RectTransform BackgroundRectTransform;
    private RectTransform targetsParent;
    private RectTransform meterRectTransform;

    private float minY;
    private float maxY;
    private int targetHeight = 20;
    private bool targetOverlap;
    private float meterSpeed = 50;
    private float meterPos = 0;
    private bool meterGoingUp = false;

    [SerializeField] private List<RectTransform> targets;
    [SerializeField] private List<RectTransform> spawnedTargets;
    protected override void Awake()
    {
        BackgroundRectTransform = minigameCanvas.transform.Find("MinigameBackground").GetComponent<RectTransform>();
        meterRectTransform = BackgroundRectTransform.transform.Find("Meter").GetComponent<RectTransform>();
        
        //base.Awake();
    }

    private new void Start()
    {
        minY = -BackgroundRectTransform.sizeDelta.y / 2;
        maxY = BackgroundRectTransform.sizeDelta.y / 2;

        base.Start();
    }


    protected override void Update()
    {
        if (!active)
            return;
       
        MeterUpdate();

        //base.Update();
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
        active = true;
        MinigameSetActive(true);
        TargetUpdate();

        ResetMinigame();
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
                Debug.Log("overlap");
                targetOverlap = false;
            }
            else
            {
                targetTransform.localPosition = spawnPos;
                spawnedTargets.Add(target);
            }
        }
        spawnedTargets.Clear();
        

    }

    protected override void ResetMinigame()
    {

    }

    protected override void MinigameSetActive(bool active)
    {

    }
}
