using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToScene(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index) != null)
        {
            SceneManager.LoadScene(index);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
