using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoSelect : MonoBehaviour
{
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    
}
