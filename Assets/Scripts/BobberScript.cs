using FMODUnity;
using UnityEngine;

public class BobberScript : MonoBehaviour
{
    [HideInInspector] public FishingMinigame fishingMinigame;
    [SerializeField] private StudioEventEmitter FMODWaterSplash;
    public float maxSpeed;
    [SerializeField] private float linearDamping = 5;
    private bool toBeDestroyed = false;
    private bool inWater = false;
    private GameObject fishCollision;
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
        if (collision.gameObject.tag.Equals("Water"))
        {
            Debug.Log("hit water");
            inWater = true;
            FMODWaterSplash.Play();
            if (toBeDestroyed)
            {
                Debug.Log("Case 1");
                Destroy(gameObject);
                fishingMinigame.bobberInstance = null;
                Destroy(fishCollision);
                fishingMinigame.isReeling = false;
                fishingMinigame.FMODReelingIn.Stop();
                fishingMinigame.fishLocation = fishCollision.transform.position;
                fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Playing);
            }
        }    
    }

    void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fish":
                if (!inWater)
                {

                    toBeDestroyed = true;
                    fishCollision = other.gameObject;
                }
                else
                {
                    Debug.Log("Case 2");
                    Destroy(gameObject);
                    fishingMinigame.bobberInstance = null;
                    Destroy(other);
                    fishingMinigame.isReeling = false;
                    fishingMinigame.FMODReelingIn.Stop();
                    fishingMinigame.fishLocation = other.transform.position;
                    fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Playing);
                }
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
        }

    }
}
