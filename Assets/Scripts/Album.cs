using FMODUnity;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Album : MonoBehaviour
{
    public static Album instance;

    private bool isOpen = false;
    private Animator anim;
    private int currentPage = 0;

    [SerializeField] private StudioEventEmitter FMODPageTurn;
    [SerializeField] private StudioEventEmitter FMODOpenAlbum;
    [SerializeField] private StudioEventEmitter FMODCloseAlbum;

    [SerializeField] private List<Page> pages;
    [SerializeField] private List<GameObject> fishes;
    [SerializeField] private float interactionCooldown;
    private bool canInteract = true;

    private List<string> addedFish;


    private void Start()
    {
        anim = GetComponent<Animator>();

        addedFish = new List<string>();
        instance = this;
    }

    public void OnAlbumButtonInteraction()
    {
        if (!canInteract)
            return;

        if (isOpen)
        {
            anim.SetTrigger("Close");
            FMODCloseAlbum.Play();
            Cursor.lockState = CursorLockMode.Locked;
            FirstPersonMovement.instance.active = true;
            FirstPersonLook.instance.active = true;
            Jump.instance.active = true;
            Crouch.instance.active = true;
        }
        else
        {
            anim.SetTrigger("Open");
            FMODOpenAlbum.Play();
            Cursor.lockState = CursorLockMode.None;
            FirstPersonLook.instance.active = false;
            FirstPersonMovement.instance.active = false;
            Crouch.instance.active = false;
            Jump.instance.active = false;
        }
        isOpen = !isOpen;

        StartCoroutine(InteractionDelay());
    }

    public void PreviousPage()
    {
        if (!canInteract)
            return;

        if (isOpen && currentPage != 0)
        {
            currentPage--;
            FMODPageTurn.Play();
        }
        UpdatePages();
        StartCoroutine(InteractionDelay());
    }

    public void NextPage()
    {
        if (!canInteract)
            return;

        if (isOpen && currentPage != pages.Count - 1)
        {
            currentPage++;
            FMODPageTurn.Play();
        }
        UpdatePages();
        StartCoroutine(InteractionDelay());
    }

    private void UpdatePages()
    {
        foreach (Page page in pages)
        {
            page.gameObject.SetActive(false);
        }
        pages[currentPage].gameObject.SetActive(true);

    }

    private IEnumerator InteractionDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactionCooldown);
        canInteract = true;
    }

    public void NewFish(FishItem fish)
    {
        if (!addedFish.Contains(fish.name))
        {
            foreach (Page p in pages)
            {
                if (p.NewFish(fish))
                {
                    break;
                }
            }
        }
    }

}
