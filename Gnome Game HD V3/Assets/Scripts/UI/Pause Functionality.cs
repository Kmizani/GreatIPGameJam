using UnityEngine;
using UnityEngine.InputSystem;

public class PauseFunctionality : MonoBehaviour
{
    private bool isPaused = false;
    private bool isOnMainMenu = true;
    [SerializeField] GameObject pauseMenu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if (!isOnMainMenu) return;
            //isPaused = !isPaused;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
   
    public void ClickedResume()
    {
        Time.timeScale = 1f;
    }

    public void IsMainMenu(bool isMainMenu)
    {
        isOnMainMenu = isMainMenu;
    }
}
