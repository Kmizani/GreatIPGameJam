using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemCollection : MonoBehaviour
{
    private Interactable _currentInteractable;
    [SerializeField] private GameObject _ui;
    [SerializeField] private Image _oldSprite;
    [SerializeField] private Sprite _newSprite;
    [SerializeField] private GameObject _vent;
    [SerializeField] private GameObject _trigger;
    [SerializeField] private GameObject _vignetteController;
    [SerializeField] private bool _lastGem = false;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _collectSFX;

    public static event Action _gemCollected;


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
        _gemCollected?.Invoke();
        StartCoroutine(ShowText());
        _oldSprite.sprite = _newSprite;
        Debug.Log("gem script");
    }

    private IEnumerator ShowText()
    {
        Debug.Log("show text");
        _source.PlayOneShot(_collectSFX);
        _ui.SetActive(true);
        yield return new WaitForSeconds(2f);
        _ui.SetActive(false);
        _vent.SetActive(false);
        if (_lastGem) _trigger.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
