using UnityEngine;

public class BaitRespawn : MonoBehaviour
{
    [SerializeField] private InventoryItemDefinition baitToRespwn;
    private GameObject bait;
    private bool baitNotInInventory;

    private void Awake()
    {
        bait = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        baitNotInInventory = !Inventory.instance.HasItem(baitToRespwn.name);

        if (!bait.activeSelf && baitNotInInventory)
        {
            Debug.Log("Respawn");
            //Instantiate(respawnBait, transform.position, transform.rotation, parent);
            bait.SetActive(true);
        }

        
    }
}
