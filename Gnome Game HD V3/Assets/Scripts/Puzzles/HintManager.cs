using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Button _hintButton;
    [SerializeField] private TextMeshProUGUI _hintText;

    private string[] _currentHints;
    private int _currentIndex = -1;

    private void Awake()
    {
        // make sure only one HintManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (_hintButton != null)
            _hintButton.onClick.AddListener(ShowNextHint);

        if (_hintText != null)
            _hintText.text = "";
    }

    public void SetHints(string[] hints)
    {
        _currentHints = hints;
        _currentIndex = -1;
        _hintText.text = "";
        _hintButton.interactable = true;
    }

    public void ShowNextHint()
    {
        if (_currentHints == null || _currentHints.Length == 0)
        {
            _hintText.text = "No hints available.";
            return;
        }

        _currentIndex++;

        if (_currentIndex < _currentHints.Length)
        {
            _hintText.text += _currentHints[_currentIndex] + "\n\n";
        }
        else
        {
            _hintButton.interactable = false;
        }
    }

    public void ClearHints()
    {
        _currentHints = null;
        _currentIndex = -1;
        _hintText.text = "";
        _hintButton.interactable = false;
    }
}
