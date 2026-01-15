using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class HasUsableItem : MonoBehaviour
{
    [SerializeField] private List<InventoryItemDefinition> bait = new List<InventoryItemDefinition>();


    public bool CheckForItem()
    {
        foreach (var bait in bait)
        {
            Inventory.instance.HasItem(bait.name);
            int difficultyLevel = bait.dificultyLevel;

            if (Inventory.instance.HasItem(bait.name) == true)
            {
                return true;
            }
        }
        return false;
    }
}
