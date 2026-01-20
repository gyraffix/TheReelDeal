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
            if (bait != null && Inventory.instance.HasItem(bait.name))
            {
                Inventory.instance.RemoveItem(bait.name);
                return true;
            }
            else if (bait == null)
                Debug.Log("not working");
            else if (!Inventory.instance.HasItem(bait.name))
                Debug.Log("Dont have it");
        }
        return false;
    }
}
