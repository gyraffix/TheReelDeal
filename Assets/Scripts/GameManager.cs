using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager gmInstance;
    public Dictionary<string, bool> completeNPCList;

    //------GlobalVars---------------
    public List<FishItem> fishList = new List<FishItem>();
    public List<InventoryItemDefinition> inventoryList = new List<InventoryItemDefinition>();

    //------SpringIslandVars---------
    private bool firstSpringIsland = false;
    public int springDifficulty;

    //------AutumnIslandVars---------
    private bool firstAutumnIsland = false;


    //------BeachIslandVars----------
    private bool firstBeachIsland = false;
    public bool baitNPCBeach = false;

    //---------------
    public string lastScene = "";
    private DialogueRunner dr;

    private void Start()
    {
        
        if (gmInstance != null && gmInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        gmInstance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        completeNPCList = new Dictionary<string, bool>();
        dr = FindFirstObjectByType<DialogueRunner>();

        dr.StartDialogue("IntroNarration");

        completeNPCList.Add("questNPCSpring", false);
        completeNPCList.Add("baitNPCSpring", false);
        completeNPCList.Add("questNPCAutumn", false);
        completeNPCList.Add("baitNPCAutumn", false);
        completeNPCList.Add("baitNPCBeach", false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Spring Island":
                StartCoroutine(LateBoolChange("firstSpringIsland", false));
                foreach(Camera cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
                {
                    if (cam.gameObject.GetComponent<FlareLayer>() == null)
                    {
                        cam.gameObject.SetActive(false);
                    }
                }
                GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("ArriveFromAutumn").transform.position;
                GameObject.FindGameObjectWithTag("Player").transform.rotation = GameObject.Find("ArriveFromAutumn").transform.rotation;
                GameObject.Find("Dock Barrier").SetActive(false);
                if (completeNPCList["questNPCSpring"])
                {
                    NPC npc = GameObject.Find("QuestNPC").GetComponent<NPC>();
                    npc.completed = true;
                    npc.exclamationMark.SetActive(false);
                }
                if (completeNPCList["baitNPCSpring"])
                {
                    NPC npc = GameObject.Find("BaitNPC").GetComponent<NPC>();
                    npc.completed = true;
                    npc.exclamationMark.SetActive(false);
                }

                break;
            case "Autumn Island":
                StartCoroutine(LateBoolChange("firstAutumnIsland", false));
                if (completeNPCList["questNPCAutumn"])
                {
                    NPC npc = GameObject.Find("QuestNPC").GetComponent<NPC>();
                    npc.completed = true;
                    npc.exclamationMark.SetActive(false);

                }
                if (completeNPCList["baitNPCAutumn"])
                {
                    NPC npc = GameObject.Find("BaitNPC").GetComponent<NPC>();
                    npc.completed = true;
                    npc.exclamationMark.SetActive(false);
                }
                if (lastScene == "Summer Island")
                {
                    GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("ArriveFromBeach").transform.position;
                    GameObject.FindGameObjectWithTag("Player").transform.rotation = GameObject.Find("ArriveFromBeach").transform.rotation;
                    GameObject.Find("Barrier").SetActive(false);
                    
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("ArriveFromSpring").transform.position;
                    GameObject.FindGameObjectWithTag("Player").transform.rotation = GameObject.Find("ArriveFromSpring").transform.rotation;
                    
                }
                break;
            case "Beach Island":
                StartCoroutine(LateBoolChange("firstBeachIsland", false));
                if (completeNPCList["baitNPCBeach"])
                {
                    NPC npc = GameObject.Find("BaitNPC").GetComponent<NPC>();
                    npc.completed = true;
                    npc.exclamationMark.SetActive(false);
                }
                break;
        }
        dr = FindFirstObjectByType<DialogueRunner>();

        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        foreach (InventoryItemDefinition item in inventoryList)
        {
            inventory.AddInventoryItem(item);
        }
    }

    public void AddFish()
    {
        foreach (FishItem fish in fishList)
        {
            Album.instance.NewFish(fish);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator LateBoolChange(string islandName, bool newBool)
    {
        yield return new WaitForSeconds(1);

        switch (islandName)
        {
            case "Spring Island":
                firstSpringIsland = newBool;
                break;
            case "Autumn Island":
                firstAutumnIsland = newBool;
                break;
            case "Beach Island":
                firstBeachIsland = newBool;
                break;
            default:
                break;
        }
    }

    public void SwitchToScene(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index) != null)
        {
            lastScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(index);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public bool getFirstSpringIsland()
    { return firstSpringIsland; }

    public bool getFirstAutumnIsland()
    { return firstAutumnIsland; }

    public bool getFirstBeachIsland()
    { return firstBeachIsland; }
}
