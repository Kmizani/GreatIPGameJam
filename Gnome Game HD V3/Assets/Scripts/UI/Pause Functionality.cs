using UnityEngine;
using UnityEngine.InputSystem;

public class PauseFunctionality : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        SwitchActionMap("UI");
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        SwitchActionMap("Player");
    }

    public void SwitchActionMap(string mapName)
    {
        // find first PlayerInput, if we were doing local multiplayer, we would have to find them all
        PlayerInput playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null) playerInput.SwitchCurrentActionMap(mapName);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
