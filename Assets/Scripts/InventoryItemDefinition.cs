using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemDefinition", menuName = "InventoryItem", order = 1)]
public class InventoryItemDefinition : ScriptableObject
{
    public Sprite icon;

    public string name = string.Empty;

    [Range(1f, 20f)]
    public int dificultyLevel;
}