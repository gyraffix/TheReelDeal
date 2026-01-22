using UnityEngine;
using UnityEngine.UI;

public class MashingMinigame : FishingMinigame
{
    private float requiredClicksPS;
    private Slider progressSlider;
    private float progressValue = 50;
    [Header("Mashing Minigame")]
    [SerializeField] private KeyCode mashInput = KeyCode.Space;

    private new void Awake()
    {
        base.Awake();
        progressSlider = minigameCanvas.transform.Find("MashingProgress").GetComponent<Slider>();
        progressSlider.gameObject.SetActive(false);
    }

    private new void Update()
    {
        base.Update();
        if (!active)
        {
            return;
        }
        if (minigameState == MinigameState.Playing)
        {
            MinigameSetActive(true);
            throwingSlider.gameObject.SetActive(false);
            progressSlider.value = progressValue;

            if (Input.GetKeyDown(mashInput))
            {
                progressValue += (1 / requiredClicksPS) * 5;
            }

            progressValue -= requiredClicksPS * Time.deltaTime * 5;
        }

        if (progressSlider.value > 99)
        {
            Debug.Log("Victory");
            FishingSuccessful();
        }
    }


    protected override void MinigameSetActive(bool active)
    {
        if (active) progressSlider.gameObject.SetActive(true);
        
        else progressSlider.gameObject.SetActive(false);
    }

    protected override void OnActivate()
    {
        Jump.instance.active = false;
        Crouch.instance.active = false;

        SetMinigameState(MinigameState.Throwing);
        
        requiredClicksPS = Difficulties[currentDifficultyIndex].requiredClicksPerSecond;

        active = true;
    }

    protected override void ResetMinigame()
    {
        progressValue = 50;
        
        progressSlider.value = 50;

        Debug.Log(progressSlider.value);
        if (bobberInstance != null)
        {
            Destroy(bobberInstance.gameObject);
            bobberInstance = null;
        }

        FMODReelingIn.Stop();

        progressSlider.gameObject.SetActive(false);
    }
}
