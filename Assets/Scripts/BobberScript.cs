using FMODUnity;
using UnityEngine;

public class BobberScript : MonoBehaviour
{
    [HideInInspector] public FishingMinigame fishingMinigame;
    [SerializeField] private StudioEventEmitter FMODWaterSplash;
    public float maxSpeed;
    [SerializeField] private float linearDamping = 5;
    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().maxLinearVelocity = maxSpeed;
        GetComponent<Rigidbody>().linearDamping = linearDamping;
        if (collision.gameObject.tag.Equals("Terrain"))
        {
            fishingMinigame.bobberInstance = null;
            fishingMinigame.checkBobberDistance = false;
            fishingMinigame.isReeling = false;
            fishingMinigame.FMODReelingIn.Stop();
            fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Throwing);
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fish":
                Destroy(gameObject);
                fishingMinigame.bobberInstance = null;
                fishingMinigame.isReeling = false;
                fishingMinigame.FMODReelingIn.Stop();
                fishingMinigame.fishLocation = other.transform.position;
                fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Playing);
                break;
            case "Minigame": // Doesn't work. This needs to be on trigger enter. Or collide with player
                if (fishingMinigame.checkBobberDistance)
                {
                    Destroy(fishingMinigame.bobberInstance.gameObject);
                    fishingMinigame.bobberInstance = null;
                    fishingMinigame.checkBobberDistance = false;
                    fishingMinigame.isReeling = false;
                    fishingMinigame.FMODReelingIn.Stop();
                    fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Throwing);
                }
                break;
            case "Water":
                FMODWaterSplash.Play();
                break;
        }

    }
}
