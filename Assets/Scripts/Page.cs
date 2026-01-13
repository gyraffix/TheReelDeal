using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    private int usedSpots = 0;
    public bool isFull = false;
    public GameObject[] spots;



    public void NewFish(FishItem fish)
    {
        
        TMP_Text name = spots[usedSpots].transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text desc = spots[usedSpots].transform.GetChild(1).GetComponent<TMP_Text>();
        Image image = spots[usedSpots].transform.GetChild(2).GetComponent<Image>();
        
        

        name.text = fish.name;
        //desc.text = fish.desc;
        image.sprite = fish.fishPhoto;
        

        Debug.Log(spots[usedSpots].transform.GetChild(0).name);


        usedSpots++;
        if (usedSpots == 4) isFull = true;
    }
}
