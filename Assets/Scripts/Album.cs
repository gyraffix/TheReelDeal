using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Album : MonoBehaviour
{
    public static Album instance;
    
    private bool isOpen = false;
    private Animator anim;
    private int currentPage = 0;
    [SerializeField] private List<GameObject> pages;
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
        }
        else
        {
            anim.SetTrigger("Open");
            UpdateFishes();
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
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
        pages[currentPage].SetActive(true);

    }

    private void UpdateFishes()
    {

    }

}
