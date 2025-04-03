using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("FinalScene");
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
