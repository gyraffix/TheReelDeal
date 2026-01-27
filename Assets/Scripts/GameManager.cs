using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gmInstance;

    private bool firstSpringIsland = false;
    private bool firstAutumnIsland = false;
    private bool firstBeachIsland = false;
    public int lastSceneIndex = 0;

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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Spring Island":
                StartCoroutine(LateBoolChange("firstSpringIsland", false));
                break;
            case "Autumn Island":
                StartCoroutine(LateBoolChange("firstAutumnIsland", false));
                break;
            case "Beach Island":
                StartCoroutine(LateBoolChange("firstBeachIsland", false));
                break;
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
            lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
