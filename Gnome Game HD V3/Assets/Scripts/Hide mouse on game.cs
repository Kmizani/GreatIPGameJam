using UnityEngine;

public class HideMouseOnGame : MonoBehaviour
{
    [SerializeField] private GameObject[] uiObjects; // assign all UI panels here

    private void Update()
    {
        bool anyUIActive = false;

        if (uiObjects == null) return;
        // Check if any UI object is active
        foreach (GameObject ui in uiObjects)
        {
            if (ui.activeInHierarchy)
            {
                anyUIActive = true;
                break;
            }
        }

        // Show or hide cursor based on UI state
        if (anyUIActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
