using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoreScript : MonoBehaviour
{
    [SerializeField] private string[] _storyParts;
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _transition;
    private Typewriter _typewriter;
    private int _index = 0;
    public static event Action nextPart;
    public static event Action revealAllText;

    [SerializeField] private Image _holdBar; // <- UI progress bar reference
    [SerializeField] private float holdDuration = 2f;
    private float holdTime = 0f;


    private void Start()
    {
        _typewriter = this.GetComponent<Typewriter>();
        if(_storyParts != null && _typewriter != null)
        {  
            _typewriter.FullText = _storyParts[_index];
            nextPart?.Invoke();
            _index++;
        }
    }

    public void OnNextDialogue()
    {

        if (_index < _storyParts.Length)
        {
            if (!_typewriter.textFullyShown)
            {
                revealAllText?.Invoke();
                return;
            }
            _typewriter.FullText = _storyParts[_index];
            nextPart?.Invoke();
            _index++;
        }
        else
        {
            _transition.SetActive(true);
            StartCoroutine(Wait());
            SceneManager.LoadScene(_sceneName);
        }
    }


    public void OnExitDialogue()
    {
        //add a lerp from 0 - 1 of an image in .5 here for the UI or change to just press enter
        _holdBar.fillAmount = Mathf.Lerp(0, 1, holdDuration);
        _transition.SetActive(true);
            StartCoroutine(Wait());
        SceneManager.LoadScene(_sceneName);
        
    }

    private IEnumerator Load()
    {
        holdTime += Time.deltaTime;
        _holdBar.fillAmount = Mathf.Lerp(0,1, holdDuration);
        yield return null;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
    }
}
