using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _transition;
    private void OnTriggerEnter(Collider other)
    {
        if(_sceneName != null)
        {
            _transition.SetActive(true);
            StartCoroutine(Wait()); 
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(_sceneName);
    }
}
