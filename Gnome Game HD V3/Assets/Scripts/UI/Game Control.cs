using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
