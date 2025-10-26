using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class WordPuzzle : Puzzle
{
    [SerializeField] private GameObject[] UI;
    [SerializeField] private string _correctAnswer;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _position;
    [SerializeField] private float _rotZ = -90;
    private string _playerAnswer = "";
    private Interactable _currentInteractable;
    [SerializeField] private TMP_InputField[] _inputFields;
    [SerializeField] private string[] _puzzleHints;
    [SerializeField] private bool lastPuzzle = false;
    [SerializeField] private GameObject _gem;
    [SerializeField] private GameObject _vent;
    [SerializeField] private float _rotX = -90;

    private void Awake()
    {
        // Cache references early so OnEnable can safely subscribe
        _currentInteractable = GetComponent<Interactable>();

        if (UI != null)
        {
            foreach(GameObject ui in UI)
            {
                ui.SetActive(false);
            }
            // Add listeners to all input fields
            foreach (TMP_InputField field in _inputFields)
            {
                field.characterLimit = 1;
                field.onValueChanged.AddListener(delegate { OnValueChanged(field); });
            }
        }

        if (lastPuzzle) _gem.SetActive(false);
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
            _currentInteractable.triggerEvent += PuzzleLogic;

        if (HintManager.Instance != null)
            HintManager.Instance.SetHints(_puzzleHints);
    }

    private void OnDisable()
    {
        if (_currentInteractable != null)
            _currentInteractable.triggerEvent -= PuzzleLogic;

        if (HintManager.Instance != null)
            HintManager.Instance.ClearHints();
    }

    protected virtual void PuzzleLogic()
    {
        if (HintManager.Instance != null)
            HintManager.Instance.SetHints(_puzzleHints);

        foreach (GameObject ui in UI)
        {
            ui.SetActive(true);
        }
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
            foreach (GameObject ui in UI)
            {
                ui.SetActive(false);
            }
            this.GetComponent<Interactable>().HideIcon();
            this.GetComponent<Interactable>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            if (lastPuzzle)
            {
                _vent.SetActive(false);
                _gem.SetActive(true);
            }
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
            field.text = "";
            field.image.color = Color.white;
            field.DeactivateInputField();
        }
        yield return null;

        // Focus the first input field again
        if (_inputFields.Length > 0)
            _inputFields[0].ActivateInputField();
    }

    private void BookFall()
    {
        _player.transform.position = _position.position;
        StartCoroutine(RotateBook());
    }

    private IEnumerator RotateBook()
    {
        yield return new WaitForSeconds(1.5f);
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = Quaternion.Euler(0, -90, _rotZ);
        float elapsed = 0f;

        while (elapsed < 1)
        {
            transform.localRotation = Quaternion.Lerp(startRot, endRot, elapsed / 1);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = endRot; // snap to final rotation
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject ui in UI)
            {
                ui.SetActive(false);
                this.GetComponent<Interactable>().enabled = true;
            }
        }
    }

    private void VentFall()
    {
        Debug.Log("Vent Fall");
        StartCoroutine(RotateVent());
    }

    private IEnumerator RotateVent()
    {
        Quaternion startRot = _vent.transform.localRotation;
        Quaternion endRot = Quaternion.Euler(_rotX, 0, 0);
        float elapsed = 0f;

        while (elapsed < 1)
        {
            _vent.transform.localRotation = Quaternion.Lerp(startRot, endRot, elapsed / 1);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _vent.transform.localRotation = endRot; // snap to final rotation
    }
}
