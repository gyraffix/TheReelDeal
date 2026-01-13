using UnityEngine;
using UnityEngine.Events;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class OpenInventory : MonoBehaviour
{
    public bool inventory;
    public bool album;
    public bool previousPage;
    public bool nextPage;

    public UnityEvent OnInventoryButton;
    public UnityEvent OnAlbumButton;
    public UnityEvent OnPreviousPageButton;
    public UnityEvent OnNextPageButton;


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

    public void OnAlbum(InputValue value)
    {
        AlbumInput(value.isPressed);
    }

    public void AlbumInput(bool newAlbumState)
    {
        album = newAlbumState;
        if (newAlbumState)
        {
            if (OnAlbumButton != null)
            {
                OnAlbumButton.Invoke();
            }
        }
    }

    public void OnPreviousPage(InputValue value)
    {
        PreviousPageInput(value.isPressed);
    }

    public void PreviousPageInput(bool newPreviousPageState)
    {
        previousPage = newPreviousPageState;
        if (newPreviousPageState)
        {
            if (OnPreviousPageButton != null)
            {
                OnPreviousPageButton.Invoke();
            }
        }
    }
    public void OnNextPage(InputValue value)
    {
        NextPageInput(value.isPressed);
    }

    public void NextPageInput(bool newNextPageState)
    {
        previousPage = newNextPageState;
        if (newNextPageState)
        {
            if (OnNextPageButton != null)
            {
                OnNextPageButton.Invoke();
            }
        }
    }

}
