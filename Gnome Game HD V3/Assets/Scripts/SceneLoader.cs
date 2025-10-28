using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _transition;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _transitionSFX;
    private void OnTriggerEnter(Collider other)
    {
        if(_sceneName != null)
        {
            _source.PlayOneShot(_transitionSFX);
            _transition.SetActive(true);
            StartCoroutine(Wait());
            SceneManager.LoadScene(_sceneName);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
    }
}
