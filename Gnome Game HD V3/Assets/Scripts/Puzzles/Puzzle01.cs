using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Puzzle01 : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private string _correctAnswer;
    private string _playerAnswer = "";
    private Interactable _currentInteractable;
    private InputField _inputField;

    private void Awake()
    {
        // Cache references early so OnEnable can safely subscribe
        _currentInteractable = GetComponent<Interactable>();

        if (_canvas != null)
        {
            // InputField is commonly a child of the canvas
            _inputField = _canvas.GetComponentInChildren<InputField>(true);
            // make sure canvas is hidden at start
            _canvas.SetActive(false);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the interactable's event
        if (_currentInteractable != null)
            _currentInteractable.triggerEvent += WordPuzzle;
    }

    private void OnDisable()
    {
        if (_currentInteractable != null)
            _currentInteractable.triggerEvent -= WordPuzzle;
    }

    private void WordPuzzle()
    {
        if (_canvas == null)
        {
            Debug.LogWarning("Puzzle01: Canvas reference is null.");
            return;
        }

        _canvas.SetActive(true);
        Debug.Log("show canvas");

        if (_inputField != null)
        {
            // Optionally clear previous text and focus the input field
            _inputField.text = "";
            _inputField.ActivateInputField(); // focuses the field so player can type immediately
        }
        else
        {
            Debug.LogWarning("Puzzle01: InputField not found under canvas.");
        }
    }
}
