using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryView : MonoBehaviour
{
    public Inventory inventory;
    public GameObject inventoryItemPrefab;
    public GameObject itemNameSpot;
    public Transform inventoryItemContainer;
    public Transform inventoryNameContainer;
    public string openStateName = "InventoryShowAnimation";
    public string closeStateName = "InventoryHideAnimation";

    private List<GameObject> inventoryItemSlots = new List<GameObject>();
    private List<GameObject> inventoryItemNames = new List<GameObject>();
    private Animator animator;
    private bool isVisible = false;

    void Start()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory reference not set in InventoryView");
            return;
        }

        animator = GetComponent<Animator>();
        inventory.OnInventoryChanged += UpdateInventoryView;

        while (inventoryItemSlots.Count < inventory.numberOfSlots)
        {
            AddEmptyInventoryItemSlot();
        }

        UpdateInventoryView();
    }

    void AddEmptyInventoryItemSlot()
    {
        GameObject newSlot = Instantiate(inventoryItemPrefab);
        newSlot.transform.SetParent(inventoryItemContainer, false);
        
        GameObject newName = Instantiate(itemNameSpot);
        newName.transform.SetParent(inventoryNameContainer, false);
        
        inventoryItemSlots.Add(newSlot.transform.GetChild(0).gameObject);
        inventoryItemNames.Add(newName);
    }

    void UpdateInventoryView()
    {
        for (int i = 0; i < inventoryItemSlots.Count; i++)
        {
            Image image = inventoryItemSlots[i].GetComponent<Image>();
            TMP_Text name = inventoryItemNames[i].GetComponent<TMP_Text>();

            InventoryItemDefinition item = (i < inventory.GetItemCount()) ? inventory.GetItem(i) : null;
            if (item != null)
            {


                image.enabled = true;
                image.sprite = item.icon;
                inventoryItemSlots[i].name = item.name;
                name.enabled = true;
                name.text = item.name;
            }
            else
            {
                image.enabled = false;
                image.sprite = null;
                inventoryItemSlots[i].name = "";
                name.enabled = false;
                name.text = null;
            }
        }
    }

    public void OnInventoryButtonInteraction()
    {
        isVisible = !isVisible;
        if (isVisible)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    void Show()
    {
        if (animator)
        {
            animator.Play(openStateName);
        }
    }

    void Hide()
    {
        if (animator)
        {
            animator.Play(closeStateName);
        }
    }
}
