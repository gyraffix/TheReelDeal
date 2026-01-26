using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public ProximityTrigger[] connectedTriggers;

    private void Awake()
    {
        connectedTriggers = GetComponentsInChildren<ProximityTrigger>();
    }

    public void TriggerActiveChange(bool active)
    {
        if (active)
        {
            foreach (var connectedTrigger in connectedTriggers)
            {
                connectedTrigger.gameObject.SetActive(true);
            }
        }
        if (!active)
        {
            foreach (var connectedTrigger in connectedTriggers)
            {
                connectedTrigger.gameObject.SetActive(false);
            }
        }
    }
}
