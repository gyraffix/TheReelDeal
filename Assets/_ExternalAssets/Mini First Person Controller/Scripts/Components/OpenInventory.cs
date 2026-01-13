using UnityEngine;
using UnityEngine.Events;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class OpenInventory : MonoBehaviour
{
    public bool inventory;

    public UnityEvent OnInventoryButton;

    public void OnInventory(InputValue value)
    {
        InventoryInput(value.isPressed);
    }

    public void InventoryInput(bool newInventoryState)
    {
        inventory = newInventoryState;
        if (newInventoryState)
        {
            if (OnInventoryButton != null)
            {
                OnInventoryButton.Invoke();
            }
        }
    }
}
