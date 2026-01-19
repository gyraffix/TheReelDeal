using UnityEngine;

public class BobberScript : MonoBehaviour
{
    [HideInInspector] public FishingMinigame fishingMinigame;
    [SerializeField] private string fishTag = "Fish";
    [SerializeField] private float collisionSlowdown = 0.1f;
    public float maxSpeed;
    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().maxLinearVelocity = maxSpeed;

        if (collision.gameObject.tag == fishTag)
        {
            Destroy(gameObject);
            fishingMinigame.bobberInstance = null;
            fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Playing);
            FirstPersonLook.instance.active = false;
        }
    }
}
