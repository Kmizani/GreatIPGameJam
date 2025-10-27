using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemCollection : MonoBehaviour
{
    private Interactable _currentInteractable;
    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private string _text;
    [SerializeField] private Image _oldSprite;
    [SerializeField] private Sprite _newSprite;
    [SerializeField] private GameObject _vent;


    private void Awake()
    {
        _currentInteractable = GetComponent<Interactable>();
    }
    private void OnEnable()
    {
        // Subscribe to the interactable's event
        if (_currentInteractable != null)
            _currentInteractable.triggerEvent += End;
    }

    private void OnDisable()
    {
        if (_currentInteractable != null)
            _currentInteractable.triggerEvent -= End;
    }

    public void End()
    {
        StartCoroutine(ShowText());
        _oldSprite.sprite = _newSprite;
        Debug.Log("gem script");
    }

    private IEnumerator ShowText()
    {
        Debug.Log("show text");
        _textUI.text = _text;
        yield return new WaitForSeconds(3f);
        _textUI.text = "";
        _vent.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
