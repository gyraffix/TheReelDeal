using UnityEngine;

public class RotateMinigame : PlayerActivatable
{
    [SerializeField] private Canvas minigameCanvas;
    private RectTransform BackgroundRectTransform;
    private RectTransform targetRectTransform;
    private RectTransform rotator;
    private RectTransform meterRectTransform;

    private float rotationSpeed = 20f;
    private float radius;


    void Start()
    {
        radius = BackgroundRectTransform.sizeDelta.x / 2;
    }

    void Awake()
    {
        BackgroundRectTransform = minigameCanvas.transform.Find("MinigameBackground").GetComponent<RectTransform>();
        targetRectTransform = BackgroundRectTransform.transform.Find("Target").GetComponent<RectTransform>();
        rotator = BackgroundRectTransform.transform.Find("Rotator").GetComponent<RectTransform>();
        meterRectTransform = rotator.transform.Find("Meter").GetComponent<RectTransform>();
    }

    void Update()
    {
        rotator.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    protected override void OnActivate()
    {
        TargetUpdate();
    }

    void TargetUpdate()
    {
        Vector2 center = BackgroundRectTransform.transform.localPosition;
        var spawnPos = RandomPointOnCircleEdge();
        targetRectTransform.localPosition = spawnPos;
        Debug.Log(center);
        Debug.Log(spawnPos);
        
    }

    private Vector2 RandomPointOnCircleEdge()
    {        
        Vector2 spawnPos = new Vector2();        
        spawnPos = Random.insideUnitCircle.normalized * radius;
        return spawnPos;
    }
}
