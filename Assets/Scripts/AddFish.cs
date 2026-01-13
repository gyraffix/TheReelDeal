using UnityEngine;

public class AddFish : PlayerActivatable
{

    [SerializeField] private FishItem fishItem;

    protected override void OnActivate()
    {
        Album.instance.NewFish(fishItem);
        Debug.Log("activated");
    }

    



}
