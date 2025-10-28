using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }

    [SerializeField] private Button _hintButton;
    [SerializeField] private TextMeshProUGUI _hintText;

    private string[] _currentHints;
    private int _index = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // runtime fallback: try to find a button if inspector ref missing
        if (_hintButton == null)
        {
            _hintButton = FindObjectOfType<Button>();
            if (_hintButton != null)
                Debug.Log("[HintManager] Found a Button at runtime and assigned it as fallback.");
        }

        if (_hintButton != null)
            _hintButton.onClick.AddListener(ShowNextHint);
        else
            Debug.LogWarning("[HintManager] No hint button assigned or found. Hints will not be clickable.");
    }

    public void SetHints(string[] hints)
    {
        _currentHints = hints;
        _index = 0;
        if (_hintText != null) _hintText.text = "";
        if (_hintButton != null) _hintButton.interactable = hints != null && hints.Length > 0;
    }

    public void ShowNextHint()
    {
        if (_currentHints == null || _currentHints.Length == 0 || _hintText == null) return;
        _hintText.text = _currentHints[_index];
        _index++;
        if (_index >= _currentHints.Length)
            _hintButton.interactable = false;
    }

    public void ClearHints()
    {
        _currentHints = null;
        _index = 0;
        if (_hintText != null) _hintText.text = "";
        if (_hintButton != null) _hintButton.interactable = false;
    }
}
