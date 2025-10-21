using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("Lore Scene");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
