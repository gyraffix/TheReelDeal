
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    private int usedSpots = 0;
    public bool isFull = false;
    public GameObject[] spots;

    [SerializeField] private FishItem[] PlaceholderFish = new FishItem[4];
    
    private void Start()
    {
        for (int i = 0; i < 4; i++) 
        {
            TMP_Text name = spots[i].transform.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text desc = spots[i].transform.GetChild(1).GetComponent<TMP_Text>();
            Image image = spots[i].transform.GetChild(2).GetComponent<Image>();
            if (PlaceholderFish[i] != null)
            {
                name.text = PlaceholderFish[i].name;
                desc.text = PlaceholderFish[i].hint;
            }
            else
            {
                name.text = "";
                desc.text = "";
            }

            image.color = Color.clear;
        }
    }

    public bool NewFish(FishItem fish)
    {
        
        foreach (GameObject spot in spots)
        {

            TMP_Text name = spot.transform.GetChild(0).GetComponent<TMP_Text>();
            if (name.text.Equals(fish.name))
            {
                TMP_Text desc = spot.transform.GetChild(1).GetComponent<TMP_Text>();
                Image image = spot.transform.GetChild(2).GetComponent<Image>();
                desc.text = fish.desc;
                image.sprite = fish.fishPhoto;
                image.color = Color.white;
                return true;
            }

        }
        return false;
    }
}
