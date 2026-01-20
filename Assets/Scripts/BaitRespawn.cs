using UnityEngine;

public class BaitRespawn : MonoBehaviour
{
    [SerializeField] private InventoryItemDefinition baitToRespwn;
    private GameObject bait;
    private bool baitNotInInventory;
    private Collectable collectable;

    private void Awake()
    {
        bait = transform.GetChild(0).gameObject;
        collectable = transform.GetComponentInChildren<Collectable>();
    }

    private void Update()
    {
        baitNotInInventory = !Inventory.instance.HasItem(baitToRespwn.name);

        if (!bait.activeSelf && baitNotInInventory)
        {
            //Instantiate(respawnBait, transform.position, transform.rotation, parent);
            bait.SetActive(true);
            foreach (var connectedTriggers in collectable.connectedTriggers)
            {
                connectedTriggers.gameObject.SetActive(true);
            }
        }

        
    }
}
