using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemDefinition", menuName = "InventoryItem", order = 1)]
public class InventoryItemDefinition : ScriptableObject
{
    public enum BaitType { ChocolateBird, RainbowGlass, SweetStrawberry}
    public BaitType baitType;

    public Sprite icon;

    public string name = string.Empty;

    //public int itemCount = 0;

    [Range(1f, 20f)]
    private int dificultyLevel;
}