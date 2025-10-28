using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    [SerializeField] private GameObject pause;
    public void OnPlay()
    {
        SceneManager.LoadScene("Lore Scene");
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
