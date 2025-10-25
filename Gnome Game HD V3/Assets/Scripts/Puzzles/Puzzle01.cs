using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Puzzle01 : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private string _correctAnswer;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _position;
    private string _playerAnswer = "";
    private Interactable _currentInteractable;
    [SerializeField] private TMP_InputField[] _inputFields;

    private void Awake()
    {
        // Cache references early so OnEnable can safely subscribe
        _currentInteractable = GetComponent<Interactable>();

        if (_canvas != null)
        {
            // InputField is commonly a child of the canvas
            //_inputField = _canvas.GetComponentInChildren<InputField>(true);
            // make sure canvas is hidden at start
            _canvas.SetActive(false);
            // Add listeners to all input fields
            foreach (TMP_InputField field in _inputFields)
            {
                field.characterLimit = 1;
                field.onValueChanged.AddListener(delegate { OnValueChanged(field); });
            }
        }
    }

    private void OnValueChanged(TMP_InputField currentField)
    {
        int index = System.Array.IndexOf(_inputFields, currentField);

        // If user entered a character, move to next field
        if (currentField.text.Length >= 1 && index < _inputFields.Length - 1)
        {
            _inputFields[index + 1].ActivateInputField();
        }
        else if (currentField.text.Length == 0 && index > 0)
        {
            // If user backspaces, move back
            _inputFields[index - 1].ActivateInputField();
        }

        UpdatePlayerAnswer();
        CompareAnswers();
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
        _canvas.SetActive(true);
        Debug.Log("show canvas");

        if (_inputFields != null)
        {
            // Optionally clear previous text and focus the input field
            foreach (TMP_InputField field in _inputFields)
            {
                field.text = "";
            }

            if (_inputFields.Length > 0)
                _inputFields[0].ActivateInputField();

        }
    }

    private void UpdatePlayerAnswer()
    {
        _playerAnswer = "";
        foreach (TMP_InputField field in _inputFields)
        {
            _playerAnswer += field.text;
        }

        Debug.Log("Current player answer: " + _playerAnswer);
    }

    private void CompareAnswers()
    {
        if (_playerAnswer.Length < _inputFields.Length) return;

        if(_playerAnswer.ToLower() == _correctAnswer.ToLower())
        {
            _canvas.SetActive(false);
            this.GetComponent<Interactable>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            BookFall();
        }
        else
        {
            StartCoroutine(WrongAnswer());
        }
    }

    private IEnumerator WrongAnswer()
    {
        foreach (TMP_InputField field in _inputFields)
        {
            field.image.color = Color.red;
        }
        yield return new WaitForSeconds(.75f);
        foreach (TMP_InputField field in _inputFields)
        {
            field.image.color = Color.white;
        }
    }

    public void BookFall()
    {
        _player.transform.position = _position.position;
        StartCoroutine(RotateBook());
    }

    private IEnumerator RotateBook()
    {
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = Quaternion.Euler(0, -90, -90);
        float elapsed = 0f;

        while (elapsed < 1)
        {
            transform.localRotation = Quaternion.Lerp(startRot, endRot, elapsed / 1);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = endRot; // snap to final rotation
    }
}
