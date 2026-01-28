using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToScene(int index)
    {
        GameManager.gmInstance.SwitchToScene(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
