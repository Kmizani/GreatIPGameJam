using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance;

    [SerializeField] private TextMeshProUGUI hintText;
    private string[] currentHints;
    private int hintIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        ClearHints();
    }

    public void SetHints(string[] hints)
    {
        currentHints = hints;
        hintIndex = 0;
        if (hintText != null)
            hintText.text = "";
    }

    public void ClearHints()
    {
        currentHints = null;
        hintIndex = 0;
        if (hintText != null)
            hintText.text = "";
    }

    public void ShowNextHint()
    {
        if (currentHints == null || currentHints.Length == 0 || hintText == null)
            return;

        hintText.text = currentHints[hintIndex];
        hintIndex = Mathf.Min(hintIndex + 1, currentHints.Length - 1);
    }
}
