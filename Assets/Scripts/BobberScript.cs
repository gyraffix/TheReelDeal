using UnityEngine;

public class BobberScript : MonoBehaviour
{
    [HideInInspector] public FishingMinigame fishingMinigame;
    public float maxSpeed;
    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().maxLinearVelocity = maxSpeed;
    }

    void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fish":
                Destroy(gameObject);
                fishingMinigame.bobberInstance = null;
                fishingMinigame.fishLocation = other.transform.position;
                fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Playing);
                break;
            case "Minigame":
                if (fishingMinigame.checkBobberDistance)
                {
                    Destroy(fishingMinigame.bobberInstance.gameObject);
                    fishingMinigame.bobberInstance = null;
                    fishingMinigame.checkBobberDistance = false;
                    fishingMinigame.SetMinigameState(FishingMinigame.MinigameState.Throwing);
                }
                break;
        }

    }
}
