using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if(_sceneName != null)
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
