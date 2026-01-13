using UnityEngine;

[CreateAssetMenu(fileName = "FishItem", menuName = "ScriptableObjects/FishItem")]
public class FishItem : ScriptableObject
{
    public Sprite fishPhoto;
    
    public string fishName = string.Empty; 
    public string fishDescription = string.Empty;
}
