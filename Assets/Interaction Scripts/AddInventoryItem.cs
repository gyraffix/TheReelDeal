using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInventoryItem : PlayerActivatable
{
    [Space]
    public Inventory inventory;
    public InventoryItemDefinition item;
    [SerializeField] private Canvas baitCanvas;
    private GameObject baitCollectedText;

    [System.Obsolete]
    void Start() {
        if (inventory == null) {
            inventory = FindFirstObjectByType<Inventory>();
        }
    }

    private void Awake()
    {
        baitCollectedText = baitCanvas.transform.Find("BaitCollected").gameObject;
    }


    override protected void OnActivate()
    {
        if (inventory != null) {
            inventory.AddItem(item.name);
            baitCollectedText.GetComponent<Animator>().SetTrigger("BaitCollected");
        }
    }
}
