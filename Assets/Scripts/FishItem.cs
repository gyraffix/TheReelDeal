using UnityEngine;

[CreateAssetMenu(fileName = "FishItem", menuName = "ScriptableObjects/FishItem")]
public class FishItem : ScriptableObject
{
    public Sprite fishPhoto;
    
    public string name = string.Empty; 
    public string desc = string.Empty;
}
