using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Album : MonoBehaviour
{
    public static Album instance;
    
    private bool isOpen = false;
    private Animator anim;
    private int currentPage = 0;
    private int currentPageToAddFish = 0;
    [SerializeField] private List<Page> pages;
    [SerializeField] private List<GameObject> fishes;



    private void Start()
    {
        anim = GetComponent<Animator>();

        instance = this;
    }

    


    public void OnAlbumButtonInteraction()
    {
        if (isOpen)
        {
            anim.SetTrigger("Close");
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            anim.SetTrigger("Open");
            Cursor.lockState = CursorLockMode.None;
        }
        isOpen = !isOpen;

    }

    public void PreviousPage()
    {
        if (isOpen && currentPage != 0)
        {
            currentPage--;
        }
        UpdatePages();
    }

    public void NextPage()
    {
        if (isOpen && currentPage != pages.Count-1)
        {
            currentPage++;
        }
        UpdatePages();
    }

    private void UpdatePages()
    {
        foreach (Page page in pages)
        {
            page.gameObject.SetActive(false);
        }
        pages[currentPage].gameObject.SetActive(true);

    }

    public void NewFish(FishItem fish)
    {
        if (pages[currentPageToAddFish].isFull) currentPageToAddFish++;

        pages[currentPageToAddFish].NewFish(fish);
    }

}
